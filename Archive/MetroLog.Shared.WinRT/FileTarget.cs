using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MetroLog.Layouts;
using MetroLog.Targets;
using Windows.Storage;
using Windows.Storage.Search;

namespace MetroLog
{
    public abstract class FileTarget : FileTargetBase
    {
        static StorageFolder logFolder;


        protected FileTarget(Layout layout) : base(layout)
        {
        }
      
        public static async Task<StorageFolder> EnsureInitializedAsync()
        {
            if (logFolder == null)
            {
                var root = ApplicationData.Current.LocalFolder;

                logFolder = await root.CreateFolderAsync(LogFolderName, CreationCollisionOption.OpenIfExists);
                
            }
            return logFolder;
        }

        protected override async Task<Stream> GetCompressedLogsInternal()
        {
            await EnsureInitializedAsync();
            var ms = new MemoryStream();

            await ZipFile.CreateFromDirectory(logFolder, ms);
            ms.Position = 0;

            return ms;
        }
        
        protected override Task EnsureInitialized()
        {
            return EnsureInitializedAsync();
        }

        protected sealed override async Task DoCleanup(Regex pattern, DateTime threshold)
        {

            var toDelete = (await logFolder.GetFilesAsync())
                            .Where(file => pattern.Match(file.Name).Success && file.DateCreated <= threshold)
                            .ToList();

            //Queries are still not supported in Windows Phone 8.1. Ensure temp cleanup
#if WINDOWS_PHONE_APP
            var zipPattern = new Regex(@"^Log(.*).zip$");
            toDelete.AddRange((await ApplicationData.Current.TemporaryFolder.GetFilesAsync())
                              .Where(file => zipPattern.Match(file.Name).Success));
#else
            var qo = new QueryOptions(CommonFileQuery.DefaultQuery, new [] {".zip"})
                {
                    FolderDepth = FolderDepth.Shallow,
                    UserSearchFilter = "System.FileName:~<\"Log -\""
                };

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
                    InternalLogger.Current.Warn($"Failed to delete '{file.Path}'.", ex);
                }
            }
        }

        protected sealed override async Task<LogWriteOperation> DoWriteAsync(StreamWriter streamWriter, string contents, LogEventInfo entry)
        {
            // Write contents
            await WriteTextToFileCore(streamWriter, contents).ConfigureAwait(false);

            // return...
            return new LogWriteOperation(this, entry, true);
        }

        protected abstract Task WriteTextToFileCore(StreamWriter stream, string contents);

        protected override async Task<Stream> GetWritableStreamForFile(string fileName)
        {
            var file = await logFolder.CreateFileAsync(fileName, FileNamingParameters.CreationMode == FileCreationMode.AppendIfExisting ? CreationCollisionOption.OpenIfExists : CreationCollisionOption.ReplaceExisting);

            var stream = await file.OpenStreamForWriteAsync();

            if (FileNamingParameters.CreationMode == FileCreationMode.AppendIfExisting)
            {
                // Make sure we're at the end of the stream for appending
                stream.Seek(0, SeekOrigin.End);
            }

            return stream;
        }
    }
}
