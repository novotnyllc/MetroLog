using System.Diagnostics;
using MetroLog;
using MetroLog.Layouts;
using MetroLog.Targets;
using Layout = MetroLog.Layouts.Layout;

namespace MetroLogSample.Maui.DebugTarget;

public class DebugTarget : SyncTarget
{
    public DebugTarget()
        : this(new SingleLineLayout())
    {
    }

    public DebugTarget(Layout layout)
        : base(layout)
    {
    }

    protected override void Write(LogWriteContext context, LogEventInfo entry)
    {
        var message = Layout.GetFormattedString(context, entry);

        Debug.WriteLine(message);
    }
}
