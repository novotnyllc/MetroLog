using System.Text.RegularExpressions;
using MetroLog.Internal;
using MetroLog.Layouts;
using MetroLog.Operators;

namespace MetroLog.Targets;

/// <summary>
///     Base class for file targets.
/// </summary>
public abstract class FileTargetBase : AsyncTarget, ILogCompressor
{
    protected const string LogFolderName = "MetroLogs";

    private readonly AsyncLock _lock = new();

    private readonly Dictionary<string, StreamWriter> _openStreamWriters = new();

    protected FileTargetBase(
        Layout layout,
        int retainDays,
        FileNamingParameters? fileNamingParameters,
        bool keepLogFilesOpenForWrite)
        : base(layout)
    {
        FileNamingParameters = fileNamingParameters ?? new FileNamingParameters();
        RetainDays = retainDays;
        KeepLogFilesOpenForWrite = keepLogFilesOpenForWrite;
    }

    /// <summary>
    ///     Gets an object that defines the file naming parameters.
    /// </summary>
    public FileNamingParameters FileNamingParameters { get; }

    /// <summary>
    ///     Gets or sets the number of days to retain log files for.
    /// </summary>
    public int RetainDays { get; }

    /// <summary>
    ///     Determines whether file streams remain open for further writes. This can increase perf
    ///     as the OS doesn't need to load/skip to the end of the file each write. Default is true.
    /// </summary>
    public bool KeepLogFilesOpenForWrite { get; }

    /// <summary>
    ///     Holds the next cleanup time.
    /// </summary>
    protected DateTime NextCleanupUtc { get; set; }

    public async Task CloseAllOpenFiles()
    {
        using (await _lock.LockAsync().ConfigureAwait(false))
        {
            CloseAllOpenStreamsInternal();
        }
    }

    public async Task<MemoryStream> GetCompressedLogs()
    {
        using (await _lock.LockAsync().ConfigureAwait(false))
        {
            CloseAllOpenStreamsInternal();
            await EnsureInitialized().ConfigureAwait(false);
            return await GetCompressedLogsInternal().ConfigureAwait(false);
        }
    }

    internal Task ForceCleanupAsync()
    {
        // threshold...
        var threshold = DateTime.UtcNow.AddDays(0 - RetainDays);

        // walk...
        var regex = FileNamingParameters.GetRegex();

        return DoCleanup(regex, threshold);
    }

    protected abstract Task EnsureInitialized();

    protected abstract Task DoCleanup(Regex pattern, DateTime threshold);

    protected abstract Task<LogWriteOperation> DoWriteAsync(StreamWriter fileName, string contents, LogEventInfo entry);

    protected abstract Task<MemoryStream> GetCompressedLogsInternal();

    protected abstract Task<Stream> GetWritableStreamForFile(string fileName);

    protected override sealed async Task<LogWriteOperation> WriteAsyncCore(LogWriteContext context, LogEventInfo entry)
    {
        var contents = Layout.GetFormattedString(context, entry);
        using (await _lock.LockAsync().ConfigureAwait(false))
        {
            await EnsureInitialized().ConfigureAwait(false);
            await CheckCleanupAsync().ConfigureAwait(false);

            var filename = FileNamingParameters.GetFilename(context, entry);

            var sw = await GetOrCreateStreamWriterForFile(filename).ConfigureAwait(false);

            var op = await DoWriteAsync(sw, contents, entry).ConfigureAwait(false);
            if (!KeepLogFilesOpenForWrite)
            {
                await sw.DisposeAsync().ConfigureAwait(false);
            }

            return op;
        }
    }

    private Task CheckCleanupAsync()
    {
        var now = DateTime.UtcNow;
        if (now < NextCleanupUtc || RetainDays < 1)
        {
            return Task.CompletedTask;
        }

        try
        {
            // threshold...
            var threshold = now.AddDays(0 - RetainDays);

            // walk...
            var regex = FileNamingParameters.GetRegex();

            return DoCleanup(regex, threshold);
        }
        finally
        {
            // reset...
            NextCleanupUtc = DateTime.UtcNow.AddHours(1);
        }
    }

    private async Task<StreamWriter> GetOrCreateStreamWriterForFile(string fileName)
    {
        StreamWriter? sw = null;
        if (KeepLogFilesOpenForWrite && !_openStreamWriters.TryGetValue(fileName, out sw))
        {
            var stream = await GetWritableStreamForFile(fileName).ConfigureAwait(false);

            sw = new StreamWriter(stream) { AutoFlush = true };
            _openStreamWriters.Add(fileName, sw);
        }
        else if (sw == null)
        {
            var stream = await GetWritableStreamForFile(fileName).ConfigureAwait(false);

            sw = new StreamWriter(stream) { AutoFlush = true };
            // Do not cache streams
        }

        return sw;
    }

    private void CloseAllOpenStreamsInternal()
    {
        // this must be called within the _lock
        foreach (var streamWriter in _openStreamWriters.Values)
        {
            streamWriter.Flush();
            streamWriter.Dispose();
        }

        _openStreamWriters.Clear();
    }
}