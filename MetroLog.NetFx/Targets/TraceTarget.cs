using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Layouts;

namespace MetroLog.Targets
{
    public class TraceTarget : SyncTarget
    {
        public TraceTarget()
            : this(new SingleLineLayout())
        {
        }

        public TraceTarget(Layout layout)
            : base(layout)
        {
        }

        protected override void Write(LogWriteContext context, LogEventInfo entry)
        {
            var message = Layout.GetFormattedString(context, entry);
            Trace.WriteLine(message);
        }
    }
}
