using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Layouts
{
    public class NullLayout : Layout
    {
        public NullLayout()
        {
        }

        public override string GetFormattedString(LogEventInfo info)
        {
            return string.Empty;
        }
    }
}
