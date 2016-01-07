using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Search;

using MetroLog.Layouts;
using MetroLog.Targets;

namespace MetroLog
{
    public abstract class WinRTFileTarget : FileTargetBase
    {
        private static StorageFolder _logFolder = null;

        protected WinRTFileTarget(Layout layout)
            : base(layout)
        {
        }

        public static async Task<StorageFolder> EnsureInitializedAsync()
        {
            if (_logFolder == null)
            {
                var root = ApplicationData.Current.LocalFolder;

                _logFolder = await root.CreateFolderAsync(LogFolderName, CreationCollisionOption.OpenIfExists);
            }
            return _logFolder;
        }

        protected override async Task<Stream> GetCompressedLogsInternal()
        {
            var ms = new MemoryStream();

            await ZipFile.CreateFromDirectory(_logFolder, ms);
            ms.Position = 0;

            return ms;
        }

        protected override Task EnsureInitialized()
        {
            return EnsureInitializedAsync();
        }

        protected override sealed async Task DoCleanup(Regex pattern, DateTime threshold)
        {
            var toDelete = new List<StorageFile>();
            foreach (var file in await _logFolder.GetFilesAsync())
            {
                if (pattern.Match(file.Name).Success && file.DateCreated <= threshold)
                {
                    toDelete.Add(file);
                }
            }

            //Queries are still not supported in Windows Phone 8.1. Ensure temp cleanup
#if WINDOWS_PHONE_APP
            var zipPattern = new Regex(@"^Log(.*).zip$");
            foreach (var file in await ApplicationData.Current.TemporaryFolder.GetFilesAsync())
            {
                if (zipPattern.Match(file.Name).Success)
                    toDelete.Add(file);
            }
#else
            var qo = new QueryOptions(CommonFileQuery.DefaultQuery, new[] { ".zip" }) { FolderDepth = FolderDepth.Shallow, UserSearchFilter = "System.FileName:~<\"Log -\"" };

            var query = ApplicationData.Current.TemporaryFolder.CreateFileQueryWithOptions(qo);

            var oldLogs = await query.GetFilesAsync();
            toDelete.AddRange(oldLogs);
#endif
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

        protected override sealed async Task<LogWriteOperation> DoWriteAsync(string fileName, string contents, LogEventInfo entry)
        {
            // write...
            var creationCollisionOption = this.FileNamingParameters.CreationMode == FileCreationMode.AppendIfExisting ? CreationCollisionOption.OpenIfExists : CreationCollisionOption.ReplaceExisting;
            var file = await _logFolder.CreateFileAsync(fileName, creationCollisionOption);

            // Write contents
            await this.WriteTextToFileCore(file, contents);

            return new LogWriteOperation(this, entry, true);
        }

        protected abstract Task WriteTextToFileCore(IStorageFile file, string contents);

        protected override sealed LogWriteOperation DoWrite(string fileName, string contents, LogEventInfo entry)
        {
            // write...
            var creationCollisionOption = this.FileNamingParameters.CreationMode == FileCreationMode.AppendIfExisting ? CreationCollisionOption.OpenIfExists : CreationCollisionOption.ReplaceExisting;
            var file = _logFolder.CreateFileAsync(fileName, creationCollisionOption).GetResults();

            // Write contents
            this.WriteTextToFile(file, contents);

            return new LogWriteOperation(this, entry, true);
        }

        protected abstract void WriteTextToFile(IStorageFile file, string contents);
    }
}