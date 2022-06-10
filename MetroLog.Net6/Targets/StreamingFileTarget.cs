using MetroLog.Layouts;

namespace MetroLog.Targets;

/// <summary>
///     Defines a target that will append messages to a single file.
/// </summary>
public class StreamingFileTarget : FileTarget
{
    public StreamingFileTarget(
        string? logsDirectoryPath = null,
        int retainDays = 30,
        FileNamingParameters? fileNamingParameters = null,
        bool keepLogFilesOpenForWrite = true)
        : this(new SingleLineLayout(), logsDirectoryPath, retainDays, fileNamingParameters, keepLogFilesOpenForWrite)
    {
    }

    public StreamingFileTarget(
        Layout layout,
        string? logsDirectoryPath = null,
        int retainDays = 30,
        FileNamingParameters? fileNamingParameters = null,
        bool keepLogFilesOpenForWrite = true)
        : base(layout, logsDirectoryPath, retainDays, fileNamingParameters, keepLogFilesOpenForWrite)
    {
        FileNamingParameters.IncludeLevel = false;
        FileNamingParameters.IncludeLogger = false;
        FileNamingParameters.IncludeSequence = false;
        FileNamingParameters.IncludeSession = false;
        FileNamingParameters.IncludeTimestamp = FileTimestampMode.Date;
        FileNamingParameters.CreationMode = FileCreationMode.AppendIfExisting;
    }

    protected override Task WriteTextToFileCore(StreamWriter file, string contents)
    {
        return file.WriteLineAsync(contents);
    }
}
