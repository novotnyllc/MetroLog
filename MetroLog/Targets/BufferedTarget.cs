using MetroLog.Internal;
using MetroLog.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Targets
{
    public abstract class BufferedTarget : Target, ILazyFlushable
    {
        private List<LogEventInfo> Buffer { get; set; }
        private object _lock = new object();
        private int Threshold { get; set; }

        public BufferedTarget(Layout layout, int threshold)
            : base(layout)
        {
            if (threshold < 1)
                throw new ArgumentOutOfRangeException("threshold");

            this.Threshold = threshold;
            this.Buffer = new List<LogEventInfo>();
        }

        protected internal override sealed Task<LogWriteOperation> WriteAsync(LogEventInfo entry)
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
                    return FlushAsync(toFlush);
                else
                    return Task.FromResult(new LogWriteOperation(this, entry, true));
            }
            catch (Exception ex)
            {
                LogManager.LogInternal(string.Format("Failed to write to target '{0}'.", this), ex);
                return Task.FromResult(new LogWriteOperation(this, entry, false));
            }
        }

        private async Task<LogWriteOperation> FlushAsync(IEnumerable<LogEventInfo> toFlush)
        {
            try
            {
                await DoFlushAsync(toFlush);

                // this is a slight cheat in that we return the first one back, even though we may have
                // written more...
                return new LogWriteOperation(this, toFlush, true);
            }
            catch (Exception ex)
            {
                LogManager.LogInternal(string.Format("Failed to flush for target '{0}'.", this), ex);
                return new LogWriteOperation(this, toFlush, false);
            }
        }

        protected abstract Task DoFlushAsync(IEnumerable<LogEventInfo> toFlush);

        async Task ILazyFlushable.LazyFlushAsync()
        {
            List<LogEventInfo> toFlush = null;
            lock (_lock)
            {
                toFlush = new List<LogEventInfo>(this.Buffer);
                this.Buffer = new List<LogEventInfo>();
            }

            // flush...
            if(toFlush.Any())
                await DoFlushAsync(toFlush);
        }
    }
}
