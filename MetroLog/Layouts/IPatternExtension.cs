
namespace MetroLog.Layouts
{
    public interface IPatternExtension
    {
        string Placeholder { get; }

        string GetValue();

        void SetLogEventInfo(LogEventInfo logEventInfo);
    }
}
