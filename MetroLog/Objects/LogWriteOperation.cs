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
        private Task _task;

        internal LogWriteOperation(LogEventInfo entry)
        {
            this.Entry = entry;
        }

        internal LogWriteOperation(LogEventInfo entry, Task task)
            : this(entry)
        {
            _task = task;
        }

        public Task Task
        {
            get
            {
                if (_task == null)
                    _task = Task.FromResult<bool>(true);
                return _task;
            }
        }
    }
}
