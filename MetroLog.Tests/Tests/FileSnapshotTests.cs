using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Targets;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.Storage;

namespace MetroLog.Tests
{
    [TestClass]
    public class FileSnapshotTests
    {
        [TestMethod]
        public async Task TestFileSnapshot()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Fatal, new FileSnapshotTarget());

            // send through a log entry...
            var loggable = new TestLoggable();
            var op = await loggable.Fatal("Testing file write...", new InvalidOperationException("An exception message..."));

            // load the file...
            var folder = await FileSnapshotTarget.EnsureInitializedAsync();
            var files = await folder.GetFilesAsync();
            var file = files.Where(v => v.Name.Contains(op[0].Entry.SequenceID.ToString())).First();
            string contents = await FileIO.ReadTextAsync(file);

            // check...
            Assert.IsTrue(contents.Contains("Testing file write..."));
            Assert.IsTrue(contents.Contains("An exception message..."));
        }
    }
}
