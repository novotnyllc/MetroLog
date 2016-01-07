using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MetroLog.Layouts;

namespace MetroLog.Targets
{
    public abstract class BufferedTarget : AsyncTarget, ILazyFlushable
    {
        private List<LogEventInfo> buffer;

        private readonly object lockObject = new object();

        public int Threshold { get; set; }

        public BufferedTarget(Layout layout, int threshold)
            : base(layout)
        {
            if (threshold < 1)
            {
                throw new ArgumentOutOfRangeException("threshold");
            }

            this.Threshold = threshold;
            this.buffer = new List<LogEventInfo>();
        }

        protected override sealed Task<LogWriteOperation> WriteAsyncCore(LogWriteContext context, LogEventInfo entry)
        {
            try
            {
                // add...
                List<LogEventInfo> toFlush = null;
                lock (this.lockObject)
                {
                    this.buffer.Add(entry);

                    // write buffer if it has become full OR the arriving context tells us that a fatal exception happened
                    if (this.buffer.Count >= this.Threshold || context.IsFatalException)
                    {
                        toFlush = new List<LogEventInfo>(this.buffer);
                        this.buffer = new List<LogEventInfo>();
                    }
                }

                // anything to flush?
                if (toFlush != null)
                {
                    if (context.IsFatalException)
                    {
                        return Task.FromResult(this.Flush(toFlush));
                    }
                    else
                    {
                        return this.FlushAsync(toFlush);
                    }
                }
                else
                {
                    return Task.FromResult(new LogWriteOperation(this, entry, true));
                }
            }
            catch (Exception ex)
            {
                InternalLogger.Current.Error(string.Format("Failed to write to target '{0}'.", this), ex);
                return Task.FromResult(new LogWriteOperation(this, entry, false));
            }
        }

        private async Task<LogWriteOperation> FlushAsync(IEnumerable<LogEventInfo> toFlush)
        {
            try
            {
                await this.DoFlushAsync(toFlush);

                return new LogWriteOperation(this, toFlush, true);
            }
            catch (Exception ex)
            {
                InternalLogger.Current.Error(string.Format("Failed to flush for target '{0}'.", this), ex);
                return new LogWriteOperation(this, toFlush, false);
            }
        }

        protected abstract Task DoFlushAsync(IEnumerable<LogEventInfo> toFlush);


        private LogWriteOperation Flush(IEnumerable<LogEventInfo> toFlush)
        {
             try
            {
                this.DoFlush(toFlush);

                return new LogWriteOperation(this, toFlush, true);
            }
            catch (Exception ex)
            {
                InternalLogger.Current.Error(string.Format("Failed to flush for target '{0}'.", this), ex);
                return new LogWriteOperation(this, toFlush, false);
            }
        }

        protected abstract void DoFlush(IEnumerable<LogEventInfo> toFlush);

        async Task ILazyFlushable.LazyFlushAsync(LogWriteContext context)
        {
            List<LogEventInfo> toFlush = null;
            lock (this.lockObject)
            {
                toFlush = new List<LogEventInfo>(this.buffer);
                this.buffer = new List<LogEventInfo>();
            }

            if (toFlush.Any())
            {
                await this.DoFlushAsync(toFlush);
            }
        }
    }
}