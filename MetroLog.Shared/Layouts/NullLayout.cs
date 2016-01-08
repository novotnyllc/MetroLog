using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Layouts
{
    public class NullLayout : Layout
    {
        public override string GetFormattedString(LogWriteContext context, LogEventInfo info)
        {
            return string.Empty;
        }
    }
}
