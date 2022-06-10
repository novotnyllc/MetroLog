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
        static DirectoryInfo logFolder;
        string appDataPath;

        public string PathUnderAppData
        {
            get { return appDataPath; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value));

                appDataPath = Path.Combine(GetUserAppDataPath(), value);
            }
        }

        protected override Task<Stream> GetCompressedLogsInternal()
        {
            var ms = new MemoryStream();

            using (var a = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                foreach (var file in logFolder.GetFiles())
                {
                    a.CreateEntryFromFile(file.FullName, file.Name);
                }
            }

            ms.Position = 0;

            return Task.FromResult<Stream>(ms);
        }

        protected FileTarget(Layout layout) : base(layout)
        {
            appDataPath = GetUserAppDataPath();
        }

        protected override Task EnsureInitialized()
        {
            var tcs = new TaskCompletionSource<bool>();

            try
            {
                if (logFolder == null)
                {
                    var root = new DirectoryInfo(PathUnderAppData);

                    var lf = root.CreateSubdirectory(LogFolderName);


                    Interlocked.CompareExchange(ref logFolder, lf, null);
                }

                tcs.SetResult(true);
            }
            catch(Exception e)
            {
                tcs.SetException(e);
            }

            return tcs.Task;
        }

        protected sealed override async Task<LogWriteOperation> DoWriteAsync(StreamWriter streamWriter, string contents, LogEventInfo entry)
        {
            // Write contents
            await WriteTextToFileCore(streamWriter, contents);
           
            // return...
            return new LogWriteOperation(this, entry, true);
        }

        protected abstract Task WriteTextToFileCore(StreamWriter file, string contents);

        protected override Task<Stream> GetWritableStreamForFile(string fileName)
        {
            var fileMode = FileNamingParameters.CreationMode == FileCreationMode.AppendIfExisting ? FileMode.Append : FileMode.Create;

            var fs = new FileStream(Path.Combine(logFolder.FullName, fileName), fileMode, FileAccess.Write);
            if (fileMode == FileMode.Append)
            {
                // Make sure we're at the end for an apend
                fs.Seek(0, SeekOrigin.End);
            }

            return Task.FromResult<Stream>(fs);
        }

        protected sealed override Task DoCleanup(Regex pattern, DateTime threshold)
        {
            return Task.Run(() =>
                {
                    var toDelete = new List<FileInfo>();
                    foreach (var file in logFolder.EnumerateFiles())
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
