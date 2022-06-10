using MetroLog.Targets;

namespace MetroLog;

public readonly struct LogWriteOperation
{
    private readonly List<LogEventInfo> _entries;

    public LogWriteOperation(Target target, LogEventInfo entry, bool success)
        : this(target, new List<LogEventInfo> { entry }, success)
    {
    }

    public LogWriteOperation(Target target, IEnumerable<LogEventInfo> entries, bool success)
    {
        Target = target;
        _entries = new List<LogEventInfo>(entries);
        Success = success;
    }

    public Target Target { get; }

    public bool Success { get; }

    public IEnumerable<LogEventInfo> GetEntries()
    {
        return new List<LogEventInfo>(_entries);
    }
}