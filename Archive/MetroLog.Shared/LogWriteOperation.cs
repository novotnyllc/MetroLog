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
        readonly List<LogEventInfo> entries;

        public LogWriteOperation(Target target, LogEventInfo entry, bool success)
            : this(target, new List<LogEventInfo>() { entry }, success)
        {
        }

        public LogWriteOperation(Target target, IEnumerable<LogEventInfo> entries, bool success)
        {
            Target = target;
            this.entries = new List<LogEventInfo>(entries);
            Success = success;
        }

        public Target Target { get; }

        public IEnumerable<LogEventInfo> GetEntries()
        {
            return new List<LogEventInfo>(entries);
        }

        public bool Success { get; }
    }
}
