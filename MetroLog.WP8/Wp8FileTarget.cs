using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MetroLog.Layouts;
using Windows.Storage;
using Windows.Storage.Search;

using Windows.Storage.Streams;
using System.IO.Compression;
using System.Runtime.InteropServices.WindowsRuntime;

namespace MetroLog.Targets
{
    public abstract class Wp8FileTarget : FileTargetBase
    {
        private static StorageFolder _logFolder = null;

        protected Wp8FileTarget(Layout layout)
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

        private async static Task<byte[]> ReadStorageFileToByteBuffer(IStorageFile storageFile)
        {
            IRandomAccessStream accessStream = await storageFile.OpenReadAsync();
            byte[] content = null;

            using (Stream stream = accessStream.AsStreamForRead((int)accessStream.Size))
            {
                content = new byte[stream.Length];
                await stream.ReadAsync(content, 0, (int)stream.Length);
            }

            return content;
        }

        private string GetCompressedFileName(string baseDirPath, StorageFile file)
        {
            return file.Path.Remove(0, baseDirPath.Length);
        }

        private async Task ZipFolderContents(StorageFolder sourceFolder, ZipArchive archive, string baseDirPath)
        {
            IReadOnlyList<StorageFile> files = await sourceFolder.GetFilesAsync();
            Regex pattern = FileNamingParameters.GetRegex();

            foreach (StorageFile file in files)
            {
                if (pattern.Match(file.Name).Success)
                {
                    ZipArchiveEntry readmeEntry = archive.CreateEntry(GetCompressedFileName(baseDirPath, file));

                    byte[] buffer = await ReadStorageFileToByteBuffer(file);

                    Stream entryStream = readmeEntry.Open();
                    await entryStream.WriteAsync(buffer, 0, buffer.Length);
                    await entryStream.FlushAsync();
                    entryStream.Dispose();
                    //using (Stream entryStream = readmeEntry.Open())
                    //{
                    //    await entryStream.WriteAsync(buffer, 0, buffer.Length);
                    //}
                }
            }

        }

        protected async override Task<Stream> GetCompressedLogsInternal()
        {
            String logFileName = "Logs-Dump.zip";
            
            // create log file and output stream
            StorageFile zippedStorageFile = await _logFolder.CreateFileAsync(logFileName, CreationCollisionOption.ReplaceExisting);
            Stream logoutputStream = await zippedStorageFile.OpenStreamForWriteAsync();
           
            // archive 
            ZipArchive zipArchive = new ZipArchive(logoutputStream, ZipArchiveMode.Create, false);
            await ZipFolderContents(_logFolder, zipArchive, logFileName);
            
            // release outfile stream
            await logoutputStream.FlushAsync();
            zipArchive.Dispose();
            logoutputStream.Dispose();

            // get inputstream for reading
            Stream loginputStream = await _logFolder.OpenStreamForReadAsync(logFileName);
          
            return loginputStream;
        }

        protected override Task EnsureInitialized()
        {
            return EnsureInitializedAsync();
        }

        sealed protected override async Task DoCleanup(Regex pattern, DateTime threshold)
        {

            Regex zipPattern = new Regex(@"^Log(.*).zip$");
            var toDelete = new List<StorageFile>();

            foreach (var file in await _logFolder.GetFilesAsync())
            {
                if (pattern.Match(file.Name).Success && file.DateCreated <= threshold)
                    toDelete.Add(file);

                if (zipPattern.Match(file.Name).Success)
                    toDelete.Add(file);
            }


            // QueryOptions class is not available in Windows Phone 8 

            /* 
            var qo = new queryoptions(commonfilequery.defaultquery, new[] { ".zip" })
            {
                folderdepth = folderdepth.shallow,
                usersearchfilter = "system.filename:~<\"log -\""
            };

            var query = applicationdata.current.temporaryfolder.createfilequerywithoptions(qo);

            var oldlogs = await query.getfilesasync();
            todelete.addrange(oldlogs);
            */

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
