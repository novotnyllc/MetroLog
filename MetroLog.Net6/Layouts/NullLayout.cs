namespace MetroLog.Layouts;

public class NullLayout : Layout
{
    public override string GetFormattedString(LogWriteContext context, LogEventInfo info)
    {
        return string.Empty;
    }
}