using MetroLog.Internal;
using MetroLog.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Targets
{
    public abstract class BufferedTarget : AsyncTarget, ILazyFlushable
    {
        List<LogEventInfo> Buffer { get; set; }
        readonly object _lock = new object();
        int Threshold { get; set; }

        public BufferedTarget(Layout layout, int threshold)
            : base(layout)
        {
            if (threshold < 1)
                throw new ArgumentOutOfRangeException(nameof(threshold));

            Threshold = threshold;
            Buffer = new List<LogEventInfo>();
        }

        protected sealed override Task<LogWriteOperation> WriteAsyncCore(LogWriteContext context, LogEventInfo entry)
        {
            try
            {
                // add...
                List<LogEventInfo> toFlush = null;
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
                if (toFlush != null)
                    return FlushAsync(context, toFlush);
                else
                    return Task.FromResult(new LogWriteOperation(this, entry, true));
            }
            catch (Exception ex)
            {
                InternalLogger.Current.Error($"Failed to write to target '{this}'.", ex);
                return Task.FromResult(new LogWriteOperation(this, entry, false));
            }
        }

        async Task<LogWriteOperation> FlushAsync(LogWriteContext context, IEnumerable<LogEventInfo> toFlush)
        {
            try
            {
                await DoFlushAsync(context, toFlush);

                // this is a slight cheat in that we return the first one back, even though we may have
                // written more...
                return new LogWriteOperation(this, toFlush, true);
            }
            catch (Exception ex)
            {
                InternalLogger.Current.Error($"Failed to flush for target '{this}'.", ex);
                return new LogWriteOperation(this, toFlush, false);
            }
        }

        protected abstract Task DoFlushAsync(LogWriteContext context, IEnumerable<LogEventInfo> toFlush);

        async Task ILazyFlushable.LazyFlushAsync(LogWriteContext context)
        {
            List<LogEventInfo> toFlush = null;
            lock (_lock)
            {
                toFlush = new List<LogEventInfo>(Buffer);
                Buffer = new List<LogEventInfo>();
            }

            // flush...
            if(toFlush.Any())
                await DoFlushAsync(context, toFlush);
        }
    }
}
