using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Targets;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MetroLog.Tests
{
    [TestClass]
    public class FileSnapshotTests
    {
        [TestMethod]
        public void TestFileSnapshot()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Fatal, new FileSnapshotTarget());

            // wait until the setup is finished...
            FileSnapshotTarget.BlockUntilSetup();

            // send through a log entry...
            var loggable = new TestLoggable();
            var entry = loggable.Fatal("Testing file write...", new InvalidOperationException("An exception message..."));

            // wait until it's finished...
            entry.WaitUntilWritten();

            // get the file...
            var folder = FileSnapshotTarget.LogFolder;
            var filesTask = folder.GetFilesAsync().AsTask();
            filesTask.Wait();
            var files = filesTask.Result;
            var file = files.Where(v => v.Name.Contains(entry.SequenceID.ToString())).First();

            // get the contents...
            var streamTask = file.OpenStreamForWriteAsync();
            streamTask.Wait();
            string contents = null;
            using (var stream = streamTask.Result)
            {
                var reader = new StreamReader(stream);
                contents = reader.ReadToEnd();
            }

            // check...
            Assert.IsTrue(contents.Contains("Testing file write..."));
            Assert.IsTrue(contents.Contains("An exception message..."));
        }
    }
}
