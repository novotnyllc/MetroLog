using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MetroLog.Layouts;
using MetroLog.Targets;
using Windows.Storage;

namespace MetroLog
{
    public abstract class WinRTFileTarget : FileTargetBase
    {
        private static StorageFolder _logFolder = null;


        protected WinRTFileTarget(Layout layout) : base(layout)
        {
        }

        public static async Task<StorageFolder> EnsureInitializedAsync()
        {
            var folder = _logFolder;
            if (folder == null)
            {
                StorageFolder logFolder = null;
                var root = ApplicationData.Current.LocalFolder;
                try
                {
                    logFolder = await root.GetFolderAsync(LogFolderName);
                }
                catch (FileNotFoundException)
                {
                }

                // if...
                if (logFolder == null)
                {
                    try
                    {
                        logFolder = await root.CreateFolderAsync(LogFolderName, CreationCollisionOption.OpenIfExists);
                    }
                    catch (Exception)
                    {
                    }

                    // if we get into trouble here, try and load it again... (something else should have created it)...
                    if (logFolder == null)
                        logFolder = await root.GetFolderAsync(LogFolderName);
                }

                // store it - but only if we have one...
                if (logFolder != null)
                    Interlocked.CompareExchange<StorageFolder>(ref _logFolder, logFolder, null);
            }
            return _logFolder;
        }


        protected override Task EnsureInitialized()
        {
            return EnsureInitializedAsync();
        }

        sealed protected override async Task DoCleanup(Regex pattern, DateTime threshold)
        {

            var toDelete = new List<StorageFile>();
            foreach (var file in await _logFolder.GetFilesAsync())
            {
                if (pattern.Match(file.Name).Success && file.DateCreated <= threshold)
                    toDelete.Add(file);
            }

            // walk...
            foreach (var file in toDelete)
            {
                try
                {
                    await file.DeleteAsync();
                }
                catch (Exception ex)
                {
                    InternalLogger.Current.Warn(string.Format("Failed to delete '{0}'.", file.Path), ex);
                }
            }
        }

        protected sealed override async Task<LogWriteOperation> DoWriteAsync(string fileName, string contents, LogEventInfo entry)
        {
            // write...

            var file = await _logFolder.CreateFileAsync(fileName, FileNamingParameters.CreationMode == FileCreationMode.AppendIfExisting ? CreationCollisionOption.OpenIfExists : CreationCollisionOption.ReplaceExisting);

            // Write contents
            await WriteTextToFileCore(file, contents);

            // return...
            return new LogWriteOperation(this, entry, true);
        }

        protected abstract Task WriteTextToFileCore(IStorageFile file, string contents);
    }
}
