#define DEBUG // here to enable the debug.writeline

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
#if REF_ASSM
            throw new InvalidOperationException("Cannot use ref assm at runtime");
#elif WINDOWS_PHONE_APP || WINDOWS_PHONE || NETFX_CORE || DOTNET
            var message = Layout.GetFormattedString(context, entry);
            Debug.WriteLine(message);
#else
            var message = Layout.GetFormattedString(context, entry);
            Trace.WriteLine(message);
#endif
        }
    }
}
