using MetroLog.Layouts;

namespace MetroLog.Targets;

public interface ILogOperator {}

public abstract class Target
{
    protected Target(Layout layout)
    {
        Layout = layout;
    }

    protected Layout Layout { get; }

    internal async Task<LogWriteOperation> WriteAsync(LogWriteContext context, LogEventInfo entry)
    {
        return await WriteAsyncCore(context, entry).ConfigureAwait(false);
    }

    protected abstract Task<LogWriteOperation> WriteAsyncCore(LogWriteContext context, LogEventInfo entry);
}
