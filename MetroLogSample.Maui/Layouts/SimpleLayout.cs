using MetroLog;
using Layout = MetroLog.Layouts.Layout;

namespace MetroLogSample.Maui.Layouts
{
    public class SimpleLayout : Layout
    {
        public override string GetFormattedString(LogWriteContext context, LogEventInfo info)
        {
            return $"{info.TimeStamp:G} - {info.Level}: {info.Message}";
        }
    }
}