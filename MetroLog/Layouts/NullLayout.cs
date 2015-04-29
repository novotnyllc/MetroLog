
namespace MetroLog.Layouts
{
    public class NullLayout : Layout
    {
        public NullLayout()
        {
        }

        public override string GetFormattedString(LogWriteContext context, LogEventInfo logEventInfo)
        {
            return string.Empty;
        }
    }
}
