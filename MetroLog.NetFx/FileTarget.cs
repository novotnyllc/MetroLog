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
        static DirectoryInfo _logFolder;
        string _appDataPath;

        public string PathUnderAppData
        {
            get { return _appDataPath; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value));

                _appDataPath = Path.Combine(GetUserAppDataPath(), value);
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
            var fileMode = FileNamingParameters.CreationMode == FileCreationMode.AppendIfExisting ? FileMode.Append :  FileMode.Create;

            // Create writer
            using (var file = new StreamWriter(new FileStream(Path.Combine(_logFolder.FullName, fileName), fileMode, FileAccess.ReadWrite)))
            {
                // Write contents
                await WriteTextToFileCore(file, contents);
                await file.FlushAsync();
            }
           
            // return...
            return new LogWriteOperation(this, entry, true);
        }

        protected abstract Task WriteTextToFileCore(StreamWriter file, string contents);

        protected sealed override Task DoCleanup(Regex pattern, DateTime threshold)
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
                            InternalLogger.Current.Warn($"Failed to delete '{file.FullName}'.", ex);
                        }
                    }
                });
        }

        static string GetUserAppDataPath()
        {
#if __ANDROID__
                var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#elif __IOS__
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var localAppData = Path.Combine(documents, "..", "Library");
#elif DOTNET
            var isUnix = true;
            var localAppData = Environment.GetEnvironmentVariable("HOME");
            if (string.IsNullOrEmpty(localAppData))
            {
                isUnix = false;
                localAppData = Environment.GetEnvironmentVariable("LOCALAPPDATA");
            }

            if (string.IsNullOrEmpty(localAppData))
            {
                throw new InvalidOperationException("Cannot determine home directory, ensure HOME or LOCALAPPDATA is set");
            }
#else
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
#endif

            
            var dirName = string.Empty;
#if DOTNET
            //dirName = Assembly.GetEntryAssembly(); // will be back in dotnet 
            dirName = isUnix ? "." : "";
            dirName += "metrolog";
#else
            // Get the .EXE assembly
            dirName = (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetName().Name;

#endif
            // Build the User App Data Path
            var path = Path.Combine(localAppData, dirName);
            return path;
        }

    }
}
