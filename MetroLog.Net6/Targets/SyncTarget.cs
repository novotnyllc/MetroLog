using MetroLog.Layouts;

namespace MetroLog.Targets;

public abstract class SyncTarget : Target
{
    protected SyncTarget(Layout layout)
        : base(layout)
    {
    }

    protected sealed override Task<LogWriteOperation> WriteAsyncCore(LogWriteContext context, LogEventInfo entry)
    {
        try
        {
            Write(context, entry);
            return Task.FromResult(new LogWriteOperation(this, entry, true));
        }
        catch (Exception? ex)
        {
            InternalLogger.Current.Error($"Failed to write to target '{this}'.", ex);
            return Task.FromResult(new LogWriteOperation(this, entry, false));
        }
    }

    protected abstract void Write(LogWriteContext context, LogEventInfo entry);
}