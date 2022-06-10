using MetroLog.Layouts;

namespace MetroLog.Targets;

public abstract class AsyncTarget : Target
{
    protected AsyncTarget(Layout layout)
        : base(layout)
    {
    }
}