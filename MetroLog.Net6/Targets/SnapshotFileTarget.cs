using MetroLog.Layouts;

namespace MetroLog.Targets;

public class SnapshotFileTarget : FileTarget
{
    public SnapshotFileTarget(
        string? logsDirectoryPath = null,
        int retainDays = 30,
        FileNamingParameters? fileNamingParameters = null,
        bool keepLogFilesOpenForWrite = true)
        : this(new FileSnapshotLayout(), logsDirectoryPath, retainDays, fileNamingParameters, keepLogFilesOpenForWrite)
    {
    }

    public SnapshotFileTarget(
        Layout layout,
        string? logsDirectoryPath = null,
        int retainDays = 30,
        FileNamingParameters? fileNamingParameters = null,
        bool keepLogFilesOpenForWrite = true)
        : base(layout, logsDirectoryPath, retainDays, fileNamingParameters, keepLogFilesOpenForWrite)
    {
        FileNamingParameters.IncludeLevel = true;
        FileNamingParameters.IncludeLogger = true;
        FileNamingParameters.IncludeSession = false;
        FileNamingParameters.IncludeSequence = true;
        FileNamingParameters.IncludeTimestamp = FileTimestampMode.DateTime;
        FileNamingParameters.CreationMode = FileCreationMode.ReplaceIfExisting;
    }

    protected override Task WriteTextToFileCore(StreamWriter stream, string contents)
    {
        return stream.WriteAsync(contents);
    }
}
