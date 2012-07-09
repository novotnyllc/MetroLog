using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Layouts;

namespace MetroLog.Targets
{
    public abstract class Target
    {
        protected Layout Layout { get; private set; }

        protected Target(Layout layout)
        {
            this.Layout = layout;
        }

        protected internal abstract void Write(LogEventInfo entry);
    }
}
