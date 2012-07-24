using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetroLog.Layouts;
using Windows.Storage;

namespace MetroLog.Targets
{
    public class FileSnapshotTarget : AsyncTarget
    {
        public static StorageFolder _logFolder = null;

        private const string LogFolderName = "MetroLogs";

        public FileSnapshotTarget()
            : this(new FileSnapshotLayout())
        {
        }

        public FileSnapshotTarget(Layout layout)
            : base(layout)
        {
        }

        public static async Task<StorageFolder> EnsureInitializedAsync()
        {
            var folder = _logFolder;
            if(folder == null)
            {
                StorageFolder logFolder = null;
                var root = ApplicationData.Current.LocalFolder;
                try
                {
                    logFolder = await root.GetFolderAsync(LogFolderName);
                }
                catch (FileNotFoundException ex)
                {
                    SinkException(ex);
                }

                // if...
                if (logFolder == null)
                {
                    try
                    {
                        logFolder = await root.CreateFolderAsync(LogFolderName, CreationCollisionOption.OpenIfExists);
                    }
                    catch (Exception ex)
                    {
                        SinkException(ex);
                    }

                    // if we get into trouble here, try and load it again... (something else should have created it)...
                    if (logFolder == null)
                        logFolder = await root.GetFolderAsync(LogFolderName);
                }

                // store it - but only if we have one...
                if(logFolder != null)
                    Interlocked.CompareExchange<StorageFolder>(ref _logFolder, logFolder, null);
            }
            return _logFolder;
        }

        private static void SinkException(Exception ex)
        {
            // no-op - just preventing compile warnings...
        }

        protected internal override async Task<LogWriteOperation> WriteAsync(LogEventInfo entry)
        {
            var folder = await EnsureInitializedAsync();
            if (folder == null)
                return new LogWriteOperation(this, entry, false);

            // create the file...
            var filename = string.Format("Log - {0} - {1} - {2} - {3}.log", entry.Logger, entry.Level, 
                entry.TimeStamp.ToString("yyyyMMdd HHmmss"), entry.SequenceID);
            var file = await folder.CreateFileAsync(filename).AsTask();
            
            // write...
            string buf = this.Layout.GetFormattedString(entry);
            await FileIO.WriteTextAsync(file, buf);

            // return...
            return new LogWriteOperation(this, entry, true);
        }
    }
}
