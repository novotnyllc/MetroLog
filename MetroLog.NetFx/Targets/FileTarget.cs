using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using MetroLog.Layouts;

namespace MetroLog.Targets
{
    public abstract class FileTarget : FileTargetBase
    {
        private static DirectoryInfo _logFolder;
        private string _appDataPath;

        public string PathUnderAppData
        {
            get { return this._appDataPath; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("value");

                this._appDataPath = Path.Combine(GetUserAppDataPath(), value);
            }
        }

        protected override Task<Stream> GetCompressedLogsInternal()
        {
            var ms = new MemoryStream();

            using (var a = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                foreach (var file in _logFolder.GetFiles())
                {
                    a.CreateEntryFromFile(file.FullName, file.Name);
                }
            }

            ms.Position = 0;

            return Task.FromResult<Stream>(ms);
        }

        protected FileTarget(Layout layout) : base(layout)
        {
            this._appDataPath = GetUserAppDataPath();
        }

        protected override Task EnsureInitialized()
        {
            var tcs = new TaskCompletionSource<bool>();

            try
            {
                if (_logFolder == null)
                {
                    var root = new DirectoryInfo(this.PathUnderAppData);

                    var logFolder = root.CreateSubdirectory(LogFolderName);


                    Interlocked.CompareExchange(ref _logFolder, logFolder, null);
                }

                tcs.SetResult(true);
            }
            catch(Exception e)
            {
                tcs.SetException(e);
            }

            return tcs.Task;
        }

        protected sealed override async Task<LogWriteOperation> DoWriteAsync(string fileName, string contents, LogEventInfo entry)
        {
            // Create writer
            using (var file = new StreamWriter(Path.Combine(_logFolder.FullName, fileName), this.FileNamingParameters.CreationMode == FileCreationMode.AppendIfExisting, Encoding.UTF8))
            {
                // Write contents
                await this.WriteTextToFileCore(file, contents);
                await file.FlushAsync();
            }
           
            // return...
            return new LogWriteOperation(this, entry, true);
        }

        protected abstract Task WriteTextToFileCore(StreamWriter file, string contents);

        sealed protected override Task DoCleanup(Regex pattern, DateTime threshold)
        {
            return Task.Run(() =>
                {
                    var toDelete = new List<FileInfo>();
                    foreach (var file in _logFolder.EnumerateFiles())
                    {
                        if (pattern.Match(file.Name).Success && file.CreationTimeUtc <= threshold)
                            toDelete.Add(file);
                    }

                    // walk...
                    foreach (var file in toDelete)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (Exception ex)
                        {
                            InternalLogger.Current.Warn(string.Format("Failed to delete '{0}'.", file.FullName), ex);
                        }
                    }
                });
        }

        private static string GetUserAppDataPath()
        {
#if __ANDROID__
                var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#elif __IOS__
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var localAppData = Path.Combine(documents, "..", "Library");
#else
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
#endif

            var path = string.Empty;

            // Get the .EXE assembly
            var assm = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();

            // Build the User App Data Path
            path = Path.Combine(localAppData, assm.GetName().Name);

            return path;
        }

    }
}
