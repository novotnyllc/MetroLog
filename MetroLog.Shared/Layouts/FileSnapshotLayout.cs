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
        public override string GetFormattedString(LogWriteContext context, LogEventInfo info)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Sequence: ");
            builder.Append(info.SequenceID);
            builder.Append("\r\nDate/time: ");
            builder.Append(info.TimeStamp.ToString(LogManager.DateTimeFormat));
            builder.Append("\r\nLevel: ");
            builder.Append(info.Level.ToString().ToUpper());
            builder.Append("\r\nThread: ");
            builder.Append(Environment.CurrentManagedThreadId);
            builder.Append("\r\nLogger: ");
            builder.Append(info.Logger);
            builder.Append("\r\n------------------------\r\n");
            builder.Append(info.Message);

            if(info.Exception != null)
            {
                builder.Append("\r\n------------------------\r\n");
                builder.Append(info.Exception);
            }

            builder.Append("\r\n------------------------\r\n");
            builder.Append("Session: ");
            builder.Append(context.Environment.ToJson());

            return builder.ToString();
        }
    }
}
