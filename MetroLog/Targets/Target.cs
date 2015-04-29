using System.Threading.Tasks;

using MetroLog.Layouts;

namespace MetroLog.Targets
{
    public abstract class Target
    {
        public Layout Layout { get; set; }

        protected Target(Layout layout)
        {
            this.Layout = layout;
        }

        internal async Task<LogWriteOperation> WriteAsync(LogWriteContext context, LogEventInfo entry)
        {
            return await this.WriteAsyncCore(context, entry).ConfigureAwait(false);
        }

        protected abstract Task<LogWriteOperation> WriteAsyncCore(LogWriteContext context, LogEventInfo entry);
    }
}