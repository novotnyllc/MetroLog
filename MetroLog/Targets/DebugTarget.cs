using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Layouts;

namespace MetroLog.Targets
{
    public class DebugTarget : SyncTarget
    {
        public DebugTarget()
            : this(new SingleLineLayout())
        {
        }

        public DebugTarget(Layout layout)
            : base(layout)
        {
        }

        protected override void Write(LogEventInfo entry)
        {
            string message = this.Layout.GetFormattedString(entry);
            Debug.WriteLine(message);
        }
    }
}
