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

        protected internal abstract void WriteSync(LogEventInfo entry);
    }
}
