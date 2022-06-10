using System.IO.Compression;
using System.Text.RegularExpressions;
using MetroLog.Layouts;

namespace MetroLog.Targets;

public abstract class FileTarget : FileTargetBase
{
    private static DirectoryInfo? _logFolder;

    protected FileTarget(
        Layout layout,
        string? logsDirectoryPath,
        int retainDays,
        FileNamingParameters? fileNamingParameters,
        bool keepLogFilesOpenForWrite)
        : base(layout, retainDays, fileNamingParameters, keepLogFilesOpenForWrite)
    {
        LogsDirectoryPath = logsDirectoryPath ?? GetDefaultUserAppDataPath();

        InternalLogger.Current.Debug($"LogsDirectoryPath set to {LogsDirectoryPath}");
    }

    public string LogsDirectoryPath { get; }

    protected override Task<MemoryStream> GetCompressedLogsInternal()
    {
        return Task.Run(
            () =>
            {
                var ms = new MemoryStream();
                using (var a = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var file in _logFolder!.GetFiles())
                    {
                        a.CreateEntryFromFile(file.FullName, file.Name);
                    }
                }

                ms.Position = 0;
                return ms;
            });
    }

    protected override Task EnsureInitialized()
    {
        var tcs = new TaskCompletionSource<bool>();

        try
        {
            if (_logFolder == null)
            {
                var lf = Directory.Exists(LogsDirectoryPath)
                    ? new DirectoryInfo(LogsDirectoryPath)
                    : Directory.CreateDirectory(LogsDirectoryPath);

                Interlocked.CompareExchange(ref _logFolder, lf, null);
            }

            tcs.SetResult(true);
        }
        catch (Exception e)
        {
            tcs.SetException(e);
        }

        return tcs.Task;
    }

    protected sealed override async Task<LogWriteOperation> DoWriteAsync(
        StreamWriter streamWriter,
        string contents,
        LogEventInfo entry)
    {
        // Write contents
        await WriteTextToFileCore(streamWriter, contents).ConfigureAwait(false);

        // return...
        return new LogWriteOperation(this, entry, true);
    }

    protected abstract Task WriteTextToFileCore(StreamWriter file, string contents);

    protected override Task<Stream> GetWritableStreamForFile(string fileName)
    {
        var fileMode = FileNamingParameters.CreationMode == FileCreationMode.AppendIfExisting
            ? FileMode.Append
            : FileMode.Create;

        var fs = new FileStream(Path.Combine(_logFolder!.FullName, fileName), fileMode, FileAccess.Write);
        if (fileMode == FileMode.Append)
        {
            // Make sure we're at the end for an append
            fs.Seek(0, SeekOrigin.End);
        }

        return Task.FromResult<Stream>(fs);
    }

    protected sealed override Task DoCleanup(Regex pattern, DateTime threshold)
    {
        return Task.Run(
            () =>
            {
                var toDelete = new List<FileInfo>();
                foreach (var file in _logFolder!.EnumerateFiles())
                {
                    if (pattern.Match(file.Name).Success && file.CreationTimeUtc <= threshold)
                    {
                        toDelete.Add(file);
                    }
                }

                // walk...
                foreach (var file in toDelete)
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception? ex)
                    {
                        InternalLogger.Current.Warn($"Failed to delete '{file.FullName}'.", ex);
                    }
                }
            });
    }

    private static string GetDefaultUserAppDataPath()
    {
#if ANDROID
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#elif IOS || MACCATALYST
        var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var localAppData = Path.Combine(documents, "..", "Library");
#else
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
#endif

        var dirName = Path.Combine(localAppData, LogFolderName);
        return dirName;
    }
}
