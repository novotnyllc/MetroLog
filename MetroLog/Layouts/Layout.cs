using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Layouts
{
    public abstract class Layout
    {
        protected Layout()
        {
        }

        protected internal abstract string GetFormattedString(LogEventInfo info);
    }
}
