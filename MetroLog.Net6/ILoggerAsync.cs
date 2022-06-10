namespace MetroLog;

/// <summary>
///     Async version of the ILogger interface.
/// </summary>
/// <remarks>
///     Whilst everything in MetroLog happens asynchronously, the tasks and returned back to the
///     caller to make the API easier to work with. However, if you need to get hold the
/// </remarks>
public interface ILoggerAsync
{
    string Name { get; }

    bool IsTraceEnabled { get; }

    bool IsDebugEnabled { get; }

    bool IsInfoEnabled { get; }

    bool IsWarnEnabled { get; }

    bool IsErrorEnabled { get; }

    bool IsFatalEnabled { get; }

    Task<LogWriteOperation[]> TraceAsync(string message, Exception? ex = null);

    Task<LogWriteOperation[]> TraceAsync(string message, params object[] ps);

    Task<LogWriteOperation[]> DebugAsync(string message, Exception? ex = null);

    Task<LogWriteOperation[]> DebugAsync(string message, params object[] ps);

    Task<LogWriteOperation[]> InfoAsync(string message, Exception? ex = null);

    Task<LogWriteOperation[]> InfoAsync(string message, params object[] ps);

    Task<LogWriteOperation[]> WarnAsync(string message, Exception? ex = null);

    Task<LogWriteOperation[]> WarnAsync(string message, params object[] ps);

    Task<LogWriteOperation[]> ErrorAsync(string message, Exception? ex = null);

    Task<LogWriteOperation[]> ErrorAsync(string message, params object[] ps);

    Task<LogWriteOperation[]> FatalAsync(string message, Exception? ex = null);

    Task<LogWriteOperation[]> FatalAsync(string message, params object[] ps);

    Task<LogWriteOperation[]> LogAsync(LogLevel logLevel, string message, Exception? ex);

    Task<LogWriteOperation[]> LogAsync(LogLevel logLevel, string message, params object[] ps);

    bool IsEnabled(LogLevel level);
}