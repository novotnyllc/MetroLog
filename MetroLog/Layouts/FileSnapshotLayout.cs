using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Internal;

namespace MetroLog.Layouts
{
    public class FileSnapshotLayout : Layout
    {
        public override string GetFormattedString(LogWriteContext context, LogEventInfo logEventInfo)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Sequence: ");
            builder.Append(logEventInfo.SequenceID);
            builder.Append("\r\nDate/time: ");
            builder.Append(logEventInfo.TimeStamp.ToString(LogManagerBase.DateTimeFormat));
            builder.Append("\r\nLevel: ");
            builder.Append(logEventInfo.Level.ToString().ToUpper());
            builder.Append("\r\nThread: ");
            builder.Append(Environment.CurrentManagedThreadId);
            builder.Append("\r\nLogger: ");
            builder.Append(logEventInfo.Logger);
            builder.Append("\r\n------------------------\r\n");
            builder.Append(logEventInfo.Message);

            if(logEventInfo.Exception != null)
            {
                builder.Append("\r\n------------------------\r\n");
                builder.Append(logEventInfo.Exception);
            }

            builder.Append("\r\n------------------------\r\n");
            builder.Append("Session: ");
            builder.Append(context.Environment.ToJson());

            return builder.ToString();
        }
    }
}
