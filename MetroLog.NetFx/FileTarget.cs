using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MetroLog.Layouts;
using MetroLog.Targets;

namespace MetroLog
{
    public abstract class FileTarget : FileTargetBase
    {
        private static DirectoryInfo _logFolder;
        private string _appDataPath;

        public string PathUnderAppData
        {
            get { return _appDataPath; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("value");

                _appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), value);
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
            _appDataPath = GetUserAppDataPath();
        }

        protected override Task EnsureInitialized()
        {
            var tcs = new TaskCompletionSource<bool>();

            try
            {
                if (_logFolder == null)
                {
                    var root = new DirectoryInfo(PathUnderAppData);

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
            using (var file = new StreamWriter(Path.Combine(_logFolder.FullName, fileName), FileNamingParameters.CreationMode == FileCreationMode.AppendIfExisting, Encoding.UTF8))
            {
                // Write contents
                await WriteTextToFileCore(file, contents);
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
            var path = string.Empty;

            // Get the .EXE assembly
            var assm = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();

            // Build the User App Data Path
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), assm.GetName().Name);

            return path;
        }

    }
}
