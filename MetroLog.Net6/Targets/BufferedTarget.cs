using MetroLog.Layouts;

namespace MetroLog.Targets;

public abstract class BufferedTarget : AsyncTarget, ILazyFlushable
{
    private readonly object _lock = new();

    public BufferedTarget(Layout layout, int threshold)
        : base(layout)
    {
        if (threshold < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(threshold));
        }

        Threshold = threshold;
        Buffer = new List<LogEventInfo>();
    }

    private List<LogEventInfo> Buffer { get; set; }

    private int Threshold { get; }

    async Task ILazyFlushable.LazyFlushAsync(LogWriteContext context)
    {
        List<LogEventInfo> toFlush;
        lock (_lock)
        {
            toFlush = new List<LogEventInfo>(Buffer);
            Buffer = new List<LogEventInfo>();
        }

        // flush...
        if (toFlush.Any())
        {
            await DoFlushAsync(context, toFlush);
        }
    }

    protected sealed override Task<LogWriteOperation> WriteAsyncCore(LogWriteContext context, LogEventInfo entry)
    {
        try
        {
            // add...
            var toFlush = new List<LogEventInfo>();
            lock (_lock)
            {
                Buffer.Add(entry);

                // if...
                if (Buffer.Count >= Threshold)
                {
                    toFlush = new List<LogEventInfo>(Buffer);
                    Buffer = new List<LogEventInfo>();
                }
            }

            // anything to flush?
            if (toFlush.Any())
            {
                return FlushAsync(context, toFlush);
            }

            return Task.FromResult(new LogWriteOperation(this, entry, true));
        }
        catch (Exception? ex)
        {
            InternalLogger.Current.Error($"Failed to write to target '{this}'.", ex);
            return Task.FromResult(new LogWriteOperation(this, entry, false));
        }
    }

    private async Task<LogWriteOperation> FlushAsync(LogWriteContext context, IEnumerable<LogEventInfo> toFlush)
    {
        try
        {
            await DoFlushAsync(context, toFlush);

            // this is a slight cheat in that we return the first one back, even though we may have
            // written more...
            return new LogWriteOperation(this, toFlush, true);
        }
        catch (Exception? ex)
        {
            InternalLogger.Current.Error($"Failed to flush for target '{this}'.", ex);
            return new LogWriteOperation(this, toFlush, false);
        }
    }

    protected abstract Task DoFlushAsync(LogWriteContext context, IEnumerable<LogEventInfo> toFlush);
}