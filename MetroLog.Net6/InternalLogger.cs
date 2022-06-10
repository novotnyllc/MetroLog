#define DEBUG // for debug.writeline

using MetroLog.Internal;

namespace MetroLog;

public class InternalLogger : ILogger
{
    private static readonly InternalLogger CurrentValue;

    static InternalLogger()
    {
        CurrentValue = new InternalLogger();
    }

    private InternalLogger()
    {
    }

    public static ILogger Current => CurrentValue;

    public string Name => "(Internal)";

    public bool IsTraceEnabled => true;

    public bool IsDebugEnabled => true;

    public bool IsInfoEnabled => true;

    public bool IsWarnEnabled => true;

    public bool IsErrorEnabled => true;

    public bool IsFatalEnabled => true;

    public void Trace(string message, Exception? ex = null)
    {
        Log(LogLevel.Trace, message, ex);
    }

    public void Trace(string message, params object[] ps)
    {
        Log(LogLevel.Trace, message, ps);
    }

    public void Debug(string message, Exception? ex = null)
    {
        Log(LogLevel.Trace, message, ex);
    }

    public void Debug(string message, params object[] ps)
    {
        Log(LogLevel.Trace, message, ps);
    }

    public void Info(string message, Exception? ex = null)
    {
        Log(LogLevel.Trace, message, ex);
    }

    public void Info(string message, params object[] ps)
    {
        Log(LogLevel.Trace, message, ps);
    }

    public void Warn(string message, Exception? ex = null)
    {
        Log(LogLevel.Trace, message, ex);
    }

    public void Warn(string message, params object[] ps)
    {
        Log(LogLevel.Trace, message, ps);
    }

    public void Error(string message, Exception? ex = null)
    {
        Log(LogLevel.Trace, message, ex);
    }

    public void Error(string message, params object[] ps)
    {
        Log(LogLevel.Trace, message, ps);
    }

    public void Fatal(string message, Exception? ex = null)
    {
        Log(LogLevel.Trace, message, ex);
    }

    public void Fatal(string message, params object[] ps)
    {
        Log(LogLevel.Trace, message, ps);
    }

    public void Log(LogLevel logLevel, string message, Exception? ex)
    {
        var sequence = LogEventInfo.GetNextSequenceId();
        var dt = LogManager.GetDateTime().ToString(LogManager.DateTimeFormat);
        var asString = logLevel.ToString().ToUpper();
        var thread = Environment.CurrentManagedThreadId;
        var formatted = ex != null
            ? $"{sequence}|{dt}|{asString}|{thread}|{message} --> {ex}"
            : $"{sequence}|{dt}|{asString}|{thread}|{message}";

        // debug...
        System.Diagnostics.Debug.WriteLine(formatted);
    }

    public void Log(LogLevel logLevel, string message, params object[] ps)
    {
        Log(logLevel, ps.Any() ? string.Format(message, ps) : message, (Exception)null);
    }

    public bool IsEnabled(LogLevel level)
    {
        return true;
    }
}
