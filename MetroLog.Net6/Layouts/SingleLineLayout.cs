using System.Text;
using MetroLog.Internal;

namespace MetroLog.Layouts;

public class SingleLineLayout : Layout
{
    public override string GetFormattedString(LogWriteContext context, LogEventInfo info)
    {
        var builder = new StringBuilder();
        builder.Append(info.SequenceId);
        builder.Append("|");
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