using System.Text.Json;
using System.Text.Json.Serialization;
using MetroLog.Internal;

namespace MetroLog;

public class LogEventInfo
{
    private static long _globalSequenceId;

    private ExceptionWrapper? _exceptionWrapper;

    public LogEventInfo(LogLevel level, string logger, string message, Exception? ex)
    {
        Level = level;
        Logger = logger;
        Message = message;
        Exception = ex;
        TimeStamp = LogManager.GetDateTime();
        SequenceId = GetNextSequenceId();
    }

    public long SequenceId { get; set; }

    public LogLevel Level { get; set; }

    public string Logger { get; set; }

    public string Message { get; set; }

    public DateTimeOffset TimeStamp { get; set; }

    [JsonIgnore]
    public Exception? Exception { get; set; }

    public ExceptionWrapper ExceptionWrapper
    {
        get
        {
            if (_exceptionWrapper == null && Exception != null)
            {
                _exceptionWrapper = new ExceptionWrapper(Exception);
            }

            return _exceptionWrapper!;
        }
        set => _exceptionWrapper = value;
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    internal static long GetNextSequenceId()
    {
        return Interlocked.Increment(ref _globalSequenceId);
    }
}