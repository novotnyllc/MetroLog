using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Layouts;
using Windows.Storage;

namespace MetroLog.Targets
{
    public class FileSnapshotTarget : AsyncTarget
    {
        private static Task _setupTask;
        public static StorageFolder LogFolder { get; private set; }

        private const string LogFolderName = "MetroLogs";

        public FileSnapshotTarget()
            : this(new FileSnapshotLayout())
        {
        }

        public FileSnapshotTarget(Layout layout)
            : base(layout)
        {
        }

        // *only* use this method from the unit test library...
        public static void BlockUntilSetup()
        {
            _setupTask.Wait();
        }

        static FileSnapshotTarget()
        {
            // create a task to load the folder...
            _setupTask = Task<Task<StorageFolder>>.Factory.StartNew(async () =>
            {
                // get...
                var root = ApplicationData.Current.LocalFolder;
                try
                {
                    await root.CreateFolderAsync(LogFolderName);
                }
                catch (FileNotFoundException ex)
                {
                    SinkException(ex);
                }

                // load...
                return await root.GetFolderAsync(LogFolderName);

            }).ContinueWith(async (t, args) =>
            {
                // set...
                LogFolder = await t.Result;

            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private static void SinkException(FileNotFoundException ex)
        {
            // no-op - just preventing compile warnings...
        }

        protected internal override void WriteAsync(LogEventInfo entry)
        {
            StorageFolder folder = LogFolder;
            if (folder == null)
                return;

            // create the file...
            var filename = string.Format("Log - {0} - {1} - {2} - {3}.log", entry.Logger, entry.Level, 
                entry.TimeStamp.ToString("yyyyMMdd HHmmss"), entry.SequenceID);
            var fileTask = folder.CreateFileAsync(filename).AsTask();
            fileTask.Wait();
            var file = fileTask.Result;

            // write...
            string buf = this.Layout.GetFormattedString(entry);
            var streamTask = file.OpenStreamForWriteAsync();
            streamTask.Wait();
            using (var stream = streamTask.Result)
            {
                var writer = new StreamWriter(stream);
                writer.Write(buf);
                writer.Flush();
            }
        }
    }
}
