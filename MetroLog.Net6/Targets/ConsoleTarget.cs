using MetroLog.Layouts;

namespace MetroLog.Targets;

public class ConsoleTarget : SyncTarget
{
    public ConsoleTarget()
        : this(new SingleLineLayout())
    {
    }

    public ConsoleTarget(Layout layout)
        : base(layout)
    {
    }

    protected override void Write(LogWriteContext context, LogEventInfo entry)
    {
        var message = Layout.GetFormattedString(context, entry);

        Console.WriteLine(message);
    }
}