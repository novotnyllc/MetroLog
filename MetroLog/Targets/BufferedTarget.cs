using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetroLog.Layouts;

namespace MetroLog.Targets
{
    public abstract class BufferedTarget : AsyncTarget, ILazyFlushable
    {
        private List<LogEventInfo> Buffer { get; set; }
        private readonly object _lock = new object();
        public int Threshold { get; set; }

        public BufferedTarget(Layout layout, int threshold)
            : base(layout)
        {
            if (threshold < 1)
                throw new ArgumentOutOfRangeException("threshold");

            this.Threshold = threshold;
            this.Buffer = new List<LogEventInfo>();
        }

        protected override sealed Task<LogWriteOperation> WriteAsyncCore(LogWriteContext context, LogEventInfo entry)
        {
            try
            {
                // add...
                List<LogEventInfo> toFlush = null;
                lock (_lock)
                {
                    this.Buffer.Add(entry);

                    // if...
                    if (this.Buffer.Count >= this.Threshold)
                    {
                        toFlush = new List<LogEventInfo>(this.Buffer);
                        this.Buffer = new List<LogEventInfo>();
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
                InternalLogger.Current.Error(string.Format("Failed to write to target '{0}'.", this), ex);
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
            catch (Exception ex)
            {
                InternalLogger.Current.Error(string.Format("Failed to flush for target '{0}'.", this), ex);
                return new LogWriteOperation(this, toFlush, false);
            }
        }

        protected abstract Task DoFlushAsync(LogWriteContext context, IEnumerable<LogEventInfo> toFlush);

        async Task ILazyFlushable.LazyFlushAsync(LogWriteContext context)
        {
            List<LogEventInfo> toFlush = null;
            lock (_lock)
            {
                toFlush = new List<LogEventInfo>(this.Buffer);
                this.Buffer = new List<LogEventInfo>();
            }

            // flush...
            if(toFlush.Any())
                await DoFlushAsync(context, toFlush);
        }
    }
}
