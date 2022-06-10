using System.Text;
using MetroLog.Internal;

namespace MetroLog.Layouts;

public class FileSnapshotLayout : Layout
{
    public override string GetFormattedString(LogWriteContext context, LogEventInfo info)
    {
        var builder = new StringBuilder();
        builder.Append("Sequence: ");
        builder.Append(info.SequenceId);
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

        if (info.Exception != null)
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