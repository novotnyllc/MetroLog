using System;
using System.Text;
using MetroLog.Internal;

namespace MetroLog.Layouts
{
    public class SingleLineLayout : Layout
    {
        public override string GetFormattedString(LogWriteContext context, LogEventInfo logEventInfo)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(logEventInfo.SequenceID);
            builder.Append("|");
            builder.Append(logEventInfo.TimeStamp.LocalDateTime.ToString(LogManagerBase.DateTimeFormat));
            builder.Append("|");
            builder.Append(logEventInfo.Level.ToString().ToUpper());
            builder.Append("|");
            builder.Append(Environment.CurrentManagedThreadId);
            builder.Append("|");
            builder.Append(logEventInfo.Logger);
            builder.Append("|");
            builder.Append(logEventInfo.Message);
            if (logEventInfo.Exception != null)
            {
                builder.Append(" --> ");
                builder.Append(logEventInfo.Exception);
            }

            return builder.ToString();
        }
    }
}
