using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Layouts;

namespace MetroLog.Targets
{
    public abstract class Target
    {
        protected Layout Layout { get; private set; }

        protected Target(Layout layout)
        {
            Layout = layout;
        }

        internal async Task<LogWriteOperation> WriteAsync(LogWriteContext context, LogEventInfo entry)
        {
            return await WriteAsyncCore(context, entry).ConfigureAwait(false);
        }

        protected abstract Task<LogWriteOperation> WriteAsyncCore(LogWriteContext context, LogEventInfo entry);
    }
}
