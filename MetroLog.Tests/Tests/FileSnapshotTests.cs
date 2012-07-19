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
        public void TestFileSnapshot()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Fatal, new FileSnapshotTarget());

            // send through a log entry...
            var loggable = new TestLoggable();
            var op = loggable.Fatal("Testing file write...", new InvalidOperationException("An exception message..."));

            // wait...
            op.Task.Wait();

            // get the file...
            var task = Task<string>.Run(async () =>
            {
                var folder = await FileSnapshotTarget.EnsureInitializedAsync();
                var files = await folder.GetFilesAsync();
                var file = files.Where(v => v.Name.Contains(op.Entry.SequenceID.ToString())).First();

                // return...
                return await FileIO.ReadTextAsync(file);
            });
            task.Wait();

            // check...
            string contents = task.Result;
            Assert.IsTrue(contents.Contains("Testing file write..."));
            Assert.IsTrue(contents.Contains("An exception message..."));
        }
    }
}
