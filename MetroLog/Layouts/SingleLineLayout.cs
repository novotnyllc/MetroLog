using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Layouts
{
    public class SingleLineLayout : Layout
    {
        protected internal override string GetFormattedString(LogEventInfo info)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(info.TimeStamp.ToString(LogManager.DateTimeFormat));
            builder.Append("|");
            builder.Append(info.Level.ToString().ToUpper());
            builder.Append("|");
            builder.Append(Environment.CurrentManagedThreadId);
            builder.Append("|");
            builder.Append(info.Logger);
            builder.Append("|");
            builder.Append(info.Message);
            if (info.Exception != null)
            {
                builder.Append(" --> ");
                builder.Append(info.Exception);
            }

            return builder.ToString();
        }
    }
}
