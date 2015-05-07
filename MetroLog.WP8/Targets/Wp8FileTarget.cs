using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MetroLog.Layouts;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MetroLog.Targets
{
    public abstract class Wp8FileTarget : FileTargetBase
    {
        private static StorageFolder logFolder = null;

        protected Wp8FileTarget(Layout layout)
            : base(layout)
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

        private static async Task<byte[]> ReadStorageFileToByteBuffer(IStorageFile storageFile)
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

        private async Task ZipFolderContents(StorageFolder sourceFolder, ZipArchive archive, string baseDirPath)
        {
            IReadOnlyList<StorageFile> files = await sourceFolder.GetFilesAsync();
            Regex pattern = this.FileNamingParameters.GetRegex();

            foreach (StorageFile file in files)
            {
                if (pattern.Match(file.Name).Success)
                {
                    ZipArchiveEntry readmeEntry = archive.CreateEntry(file.Name);

                    byte[] buffer = await ReadStorageFileToByteBuffer(file);

                    Stream entryStream = readmeEntry.Open();
                    await entryStream.WriteAsync(buffer, 0, buffer.Length);
                    await entryStream.FlushAsync();
                    entryStream.Dispose();
                }
            }
        }

        protected override async Task<Stream> GetCompressedLogsInternal()
        {
            const string LogFileName = "Logs-Dump.zip";

            // create log file and output stream
            StorageFile zippedStorageFile = await logFolder.CreateFileAsync(LogFileName, CreationCollisionOption.ReplaceExisting);
            Stream logoutputStream = await zippedStorageFile.OpenStreamForWriteAsync();

            // archive 
            ZipArchive zipArchive = new ZipArchive(logoutputStream, ZipArchiveMode.Create, false);
            await this.ZipFolderContents(logFolder, zipArchive, LogFileName);

            // release outfile stream
            await logoutputStream.FlushAsync();
            zipArchive.Dispose();
            logoutputStream.Dispose();

            // get inputstream for reading
            Stream loginputStream = await logFolder.OpenStreamForReadAsync(LogFileName);
            return loginputStream;
        }

        protected override Task EnsureInitialized()
        {
            return EnsureInitializedAsync();
        }

        protected override sealed async Task DoCleanup(Regex pattern, DateTime threshold)
        {
            Regex zipPattern = new Regex(@"^Log(.*).zip$");
            var toDelete = new List<StorageFile>();

            foreach (var file in await logFolder.GetFilesAsync())
            {
                if (pattern.Match(file.Name).Success && file.DateCreated <= threshold)
                {
                    toDelete.Add(file);
                }

                if (zipPattern.Match(file.Name).Success)
                {
                    toDelete.Add(file);
                }
            }

            // QueryOptions class is not implemented in Windows Phone 8 
            //var qo = new queryoptions(commonfilequery.defaultquery, new[] { ".zip" })
            //{
            //    folderdepth = folderdepth.shallow,
            //    usersearchfilter = "system.filename:~<\"log -\""
            //};

            //var query = applicationdata.current.temporaryfolder.createfilequerywithoptions(qo);

            //var oldlogs = await query.getfilesasync();
            //todelete.addrange(oldlogs);

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
            var file = await logFolder.CreateFileAsync(fileName, creationCollisionOption);

            // Write contents
            await this.WriteTextToFileAsync(file, contents);

            // return...
            return new LogWriteOperation(this, entry, true);
        }

        protected abstract Task WriteTextToFileAsync(IStorageFile file, string contents);

        protected override sealed LogWriteOperation DoWrite(string fileName, string contents, LogEventInfo entry)
        {
            // write...
            var creationCollisionOption = this.FileNamingParameters.CreationMode == FileCreationMode.AppendIfExisting ? CreationCollisionOption.OpenIfExists : CreationCollisionOption.ReplaceExisting;
            var file = logFolder.CreateFileAsync(fileName, creationCollisionOption).GetResults();

            // Write contents
            this.WriteTextToFile(file, contents);

            // return...
            return new LogWriteOperation(this, entry, true);
        }

        protected abstract void WriteTextToFile(IStorageFile file, string contents);
    }
}