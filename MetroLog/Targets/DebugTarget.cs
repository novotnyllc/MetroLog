using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Layouts;

namespace MetroLog.Targets
{
    public class DebugTarget : Target
    {
        public DebugTarget()
            : this(new SingleLineLayout())
        {
        }

        public DebugTarget(Layout layout)
            : base(layout)
        {
        }

        protected internal override void Write(LogEventInfo entry)
        {
            string message = this.Layout.GetFormattedString(entry);
            Debug.WriteLine(message);
        }
    }
}
