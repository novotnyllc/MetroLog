using System.Diagnostics;
using MetroLog.Layouts;

namespace MetroLog.Targets;

public class TraceTarget : SyncTarget
{
    public TraceTarget()
        : this(new SingleLineLayout())
    {
    }

    public TraceTarget(Layout layout)
        : base(layout)
    {
    }

    protected override void Write(LogWriteContext context, LogEventInfo entry)
    {
        var message = Layout.GetFormattedString(context, entry);
        Trace.WriteLine(message);
    }
}