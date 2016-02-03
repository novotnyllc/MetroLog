using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MetroLog.Internal;
using MetroLog.Targets;
using MetroLog.Layouts;
using Windows.Storage;
using Xunit;

namespace MetroLog.NetCore.Tests
{

    public class FileStreamingTargetTests
    {
        [Fact]
        public async Task TestFileSnapshot()
        {
            var target = new StreamingFileTarget();

            // send through a log entry...
            var op = await target.WriteAsync(new LogWriteContext(),
                new LogEventInfo(LogLevel.Fatal, "TestLogger", "Testing file write...", new InvalidOperationException("An exception message...")));

            // TODO: This should be Faked! We shouldn't be writing to the disk

            // load the file...
            var folder = await StreamingFileTarget.EnsureInitializedAsync();
            var files = await folder.GetFilesAsync();
            var file = files.First(v => v.Name.Contains(op.GetEntries().First().SequenceID.ToString()));
            string contents = await FileIO.ReadTextAsync(file);

            // check...
            Assert.True(contents.Contains("Testing file write..."));
            Assert.True(contents.Contains("An exception message..."));
        }

        [Fact]
        public async Task TestGetZipFile()
        {
            var manager = LogManagerFactory.CreateLogManager() as IWinRTLogManager;

            var logger = (ILoggerAsync)manager.GetLogger("test");

            // send through a log entry...
            var op = await logger.FatalAsync("Testing file write...", new InvalidOperationException("An exception message..."));

            var file = await manager.GetCompressedLogFile();

            Assert.True(file.Name.EndsWith(".zip"));

            var target = manager.DefaultConfiguration.GetTargets().OfType<FileTargetBase>().First();

            await target.ForceCleanupAsync();

            var exceptionThrow = false;
            try
            {
                var str = await file.OpenReadAsync();
            }
            catch (FileNotFoundException)
            {
                exceptionThrow = true;
            }

            Assert.True(exceptionThrow);
        }

        [Fact]
        public async Task FileStreamingThreadSafe()
        {
            var loggingConfiguration = new LoggingConfiguration();
            loggingConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new StreamingFileTarget());
            LogManagerFactory.DefaultConfiguration = loggingConfiguration;
            var log = (ILoggerAsync)LogManagerFactory.DefaultLogManager.GetLogger<FileStreamingTargetTests>();

            var tasks = new List<Task>(100);
            for (int i = 0; i < 100; i++)
            {
                var t = log.TraceAsync("What thread am I?");
                tasks.Add(t);
            }

            await Task.WhenAll(tasks);
        }


        [Fact]
        public async Task TestCurrentManagedThreadIdMatchesCaller()
        {
            var loggingConfiguration = new LoggingConfiguration();
            var target = new TestFileTarget(new CurrentManagedThreadIdLayout());
            loggingConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, target);
            LogManagerFactory.DefaultConfiguration = loggingConfiguration;
            var log = (ILoggerAsync)LogManagerFactory.DefaultLogManager.GetLogger<FileStreamingTargetTests>();

            for (int i = 0; i < 100; i++)
            {
                var expectedThreadId = Environment.CurrentManagedThreadId;
                await log.TraceAsync("What thread am I?");
                var output = target.Stream.ToArray();
                target.Stream.Position = 0;
                var loggedThreadId = int.Parse(Encoding.UTF8.GetString(output, 0, output.Length));
                Assert.Equal(expectedThreadId, loggedThreadId);
            }
        }
    }

    class CurrentManagedThreadIdLayout : Layout
    {
        public override string GetFormattedString(LogWriteContext context, LogEventInfo info)
        {
            return Environment.CurrentManagedThreadId.ToString();
        }
    }

    class TestFileTarget : StreamingFileTarget
    {
        public readonly MemoryStream Stream = new MemoryStream();

        public TestFileTarget(Layout layout) : base(layout)
        {
        }

        protected override Task<Stream> GetWritableStreamForFile(string fileName)
        {
            return Task.FromResult<Stream>(Stream);
        }
    }
}
