using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Targets;

namespace MetroLog
{
    public struct LogWriteOperation
    {
        private readonly Target _target;
        private readonly List<LogEventInfo> _entries;
        private readonly bool _success;

        public LogWriteOperation(Target target, LogEventInfo entry, bool success)
            : this(target, new List<LogEventInfo>() { entry }, success)
        {
        }

        public LogWriteOperation(Target target, IEnumerable<LogEventInfo> entries, bool success)
        {
            _target = target;
            _entries = new List<LogEventInfo>(entries);
            _success = success;
        }

        public Target Target
        {
            get
            {
                return _target;
            }
        }

        public IEnumerable<LogEventInfo> GetEntries()
        {
            return new List<LogEventInfo>(this._entries);
        }

        public bool Success
        {
            get
            {
                return _success;
            }
        }
    }
}
