using System.Text.Json;

namespace MetroLog.Targets;

public class JsonPostWrapper
{
    internal JsonPostWrapper(ILoggingEnvironment environment, IEnumerable<LogEventInfo> events)
    {
        Environment = environment;
        Events = events.ToArray();
    }

    public ILoggingEnvironment Environment { get; }

    public LogEventInfo[] Events { get; }

    internal string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}