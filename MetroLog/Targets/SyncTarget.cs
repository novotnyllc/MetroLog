using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Layouts;

namespace MetroLog.Targets
{
    public abstract class SyncTarget : Target
    {
        protected SyncTarget(Layout layout)
            : base(layout)
        {
        }

        protected internal override sealed Task<LogWriteOperation> WriteAsync(LogEventInfo entry)
        {
            try
            {
                this.Write(entry);
                return Task.FromResult<LogWriteOperation>(new LogWriteOperation(this, entry, true));
            }
            catch (Exception ex)
            {
                LogManager.LogInternal(string.Format("Failed to write to target '{0}'.", this), ex);
                return Task.FromResult<LogWriteOperation>(new LogWriteOperation(this, entry, false));
            }
        }

        protected abstract void Write(LogEventInfo entry);
    }
}
