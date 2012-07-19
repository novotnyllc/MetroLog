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
        private Target _target;
        private LogEventInfo _entry;
        private bool _success;

        internal LogWriteOperation(Target target, LogEventInfo entry, bool success)
        {
            _target = target;
            _entry = entry;
            _success = success;
        }

        public Target Target
        {
            get
            {
                return _target;
            }
        }

        public LogEventInfo Entry
        {
            get
            {
                return _entry;
            }
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
