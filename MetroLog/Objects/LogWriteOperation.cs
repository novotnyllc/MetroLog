using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    public class LogWriteOperation
    {
        public LogEventInfo Entry { get; private set; }
        public Task Task { get; private set; }

        internal LogWriteOperation(LogEventInfo entry)
        {
            this.Entry = entry;
        }

        internal LogWriteOperation(LogEventInfo entry, Task task)
            : this(entry)
        {
            this.Task = task;
        }
    }
}
