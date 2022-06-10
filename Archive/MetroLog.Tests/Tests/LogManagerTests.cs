using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Internal;
using MetroLog.Targets;
using Xunit;

namespace MetroLog.Tests
{
    public class LogManagerBaseTests
    {
        [Fact]
        public void TestGetLogger()
        {
            var manager = new LogManager(new LoggingConfiguration());

            var logger = manager.GetLogger("Foobar");
            Assert.NotNull(logger);
        }

        [Fact]
        public void TestGetLoggerByType()
        {
            var manager = new LogManager(new LoggingConfiguration());

            var logger = manager.GetLogger<LogManagerBaseTests>();
            Assert.NotNull(logger);
        }


        [Fact]
        public void TestLoggerCreated()
        {
            bool called = false;
            var manager = new LogManager(new LoggingConfiguration());
            manager.LoggerCreated += (sender, args) =>
            {
                called = true;
            };

            // call...
            var logger = manager.GetLogger<LogManagerBaseTests>();
            
            // check...
            Assert.True(called);
        }

        [Fact]
        public async Task TestGetZipNetFile()
        {

            var manager = new LogManager(LogManagerFactory.CreateLibraryDefaultSettings());
            manager.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new StreamingFileTarget());

            var logger = (ILoggerAsync)manager.GetLogger("test");
            // send through a log entry...
            await logger.DebugAsync("Test Message");

            var str = await manager.GetCompressedLogs();

            var file =
              new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory, Environment.SpecialFolderOption.None), "logs.zip"));



            using (var stream = file.Create())
            {
                await str.CopyToAsync(stream);
            }
        }
    }
}
