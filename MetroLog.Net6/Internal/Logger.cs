using MetroLog.Targets;

namespace MetroLog.Internal;

internal class Logger : ILogger, ILoggerAsync, ILoggerQuery
{
    private static readonly Task<LogWriteOperation[]> EmptyOperations =
        Task.FromResult(Array.Empty<LogWriteOperation>());

    private readonly LoggingConfiguration _configuration;

    public Logger(string name, LoggingConfiguration config)
    {
        Name = name;

        // add a target...
        _configuration = config;
    }

    public string Name { get; }

    public bool IsTraceEnabled => IsEnabled(LogLevel.Trace);

    public bool IsDebugEnabled => IsEnabled(LogLevel.Debug);

    public bool IsInfoEnabled => IsEnabled(LogLevel.Info);

    public bool IsWarnEnabled => IsEnabled(LogLevel.Warn);

    public bool IsErrorEnabled => IsEnabled(LogLevel.Error);

    public bool IsFatalEnabled => IsEnabled(LogLevel.Fatal);

    public void Trace(string message, Exception? ex = null)
    {
        TraceAsync(message, ex);
    }

    public void Trace(string message, params object[] ps)
    {
        TraceAsync(message, ps);
    }

    public void Debug(string message, Exception? ex = null)
    {
        DebugAsync(message, ex);
    }

    public void Debug(string message, params object[] ps)
    {
        DebugAsync(message, ps);
    }

    public void Info(string message, Exception? ex = null)
    {
        InfoAsync(message, ex);
    }

    public void Info(string message, params object[] ps)
    {
        InfoAsync(message, ps);
    }

    public void Warn(string message, Exception? ex = null)
    {
        WarnAsync(message, ex);
    }

    public void Warn(string message, params object[] ps)
    {
        WarnAsync(message, ps);
    }

    public void Error(string message, Exception? ex = null)
    {
        ErrorAsync(message, ex);
    }

    public void Error(string message, params object[] ps)
    {
        ErrorAsync(message, ps);
    }

    public void Fatal(string message, Exception? ex = null)
    {
        FatalAsync(message, ex);
    }

    public void Fatal(string message, params object[] ps)
    {
        FatalAsync(message, ps);
    }

    public void Log(LogLevel logLevel, string message, Exception? ex)
    {
        LogAsync(logLevel, message, ex);
    }

    public void Log(LogLevel logLevel, string message, params object[] ps)
    {
        LogAsync(logLevel, message, ps);
    }

    public bool IsEnabled(LogLevel level)
    {
        return _configuration.GetTargets(level).Any();
    }

    Task<LogWriteOperation[]> ILoggerAsync.TraceAsync(string message, Exception? ex)
    {
        return TraceAsync(message, ex);
    }

    Task<LogWriteOperation[]> ILoggerAsync.TraceAsync(string message, params object[] ps)
    {
        return TraceAsync(message, ps);
    }

    Task<LogWriteOperation[]> ILoggerAsync.DebugAsync(string message, Exception? ex)
    {
        return DebugAsync(message, ex);
    }

    Task<LogWriteOperation[]> ILoggerAsync.DebugAsync(string message, params object[] ps)
    {
        return DebugAsync(message, ps);
    }

    Task<LogWriteOperation[]> ILoggerAsync.InfoAsync(string message, Exception? ex)
    {
        return InfoAsync(message, ex);
    }

    Task<LogWriteOperation[]> ILoggerAsync.InfoAsync(string message, params object[] ps)
    {
        return InfoAsync(message, ps);
    }

    Task<LogWriteOperation[]> ILoggerAsync.WarnAsync(string message, Exception? ex)
    {
        return WarnAsync(message, ex);
    }

    Task<LogWriteOperation[]> ILoggerAsync.WarnAsync(string message, params object[] ps)
    {
        return WarnAsync(message, ps);
    }

    Task<LogWriteOperation[]> ILoggerAsync.ErrorAsync(string message, Exception? ex)
    {
        return ErrorAsync(message, ex);
    }

    Task<LogWriteOperation[]> ILoggerAsync.ErrorAsync(string message, params object[] ps)
    {
        return ErrorAsync(message, ps);
    }

    Task<LogWriteOperation[]> ILoggerAsync.FatalAsync(string message, Exception? ex)
    {
        return FatalAsync(message, ex);
    }

    Task<LogWriteOperation[]> ILoggerAsync.FatalAsync(string message, params object[] ps)
    {
        return FatalAsync(message, ps);
    }

    Task<LogWriteOperation[]> ILoggerAsync.LogAsync(LogLevel logLevel, string message, Exception? ex)
    {
        return LogAsync(logLevel, message, ex);
    }

    Task<LogWriteOperation[]> ILoggerAsync.LogAsync(LogLevel logLevel, string message, params object[] ps)
    {
        return LogAsync(logLevel, message, ps);
    }

    public IEnumerable<Target> GetTargets()
    {
        return _configuration.GetTargets();
    }

    internal Task<LogWriteOperation[]> TraceAsync(string message, Exception? ex = null)
    {
        return LogAsync(LogLevel.Trace, message, ex);
    }

    internal Task<LogWriteOperation[]> TraceAsync(string message, params object[] ps)
    {
        return LogAsync(LogLevel.Trace, message, ps);
    }

    internal Task<LogWriteOperation[]> DebugAsync(string message, Exception? ex = null)
    {
        return LogAsync(LogLevel.Debug, message, ex);
    }

    internal Task<LogWriteOperation[]> DebugAsync(string message, params object[] ps)
    {
        return LogAsync(LogLevel.Debug, message, ps);
    }

    internal Task<LogWriteOperation[]> InfoAsync(string message, Exception? ex = null)
    {
        return LogAsync(LogLevel.Info, message, ex);
    }

    internal Task<LogWriteOperation[]> InfoAsync(string message, params object[] ps)
    {
        return LogAsync(LogLevel.Info, message, ps);
    }

    internal Task<LogWriteOperation[]> WarnAsync(string message, Exception? ex = null)
    {
        return LogAsync(LogLevel.Warn, message, ex);
    }

    internal Task<LogWriteOperation[]> WarnAsync(string message, params object[] ps)
    {
        return LogAsync(LogLevel.Warn, message, ps);
    }

    internal Task<LogWriteOperation[]> ErrorAsync(string message, Exception? ex = null)
    {
        return LogAsync(LogLevel.Error, message, ex);
    }

    internal Task<LogWriteOperation[]> ErrorAsync(string message, params object[] ps)
    {
        return LogAsync(LogLevel.Error, message, ps);
    }

    internal Task<LogWriteOperation[]> FatalAsync(string message, Exception? ex = null)
    {
        return LogAsync(LogLevel.Fatal, message, ex);
    }

    internal Task<LogWriteOperation[]> FatalAsync(string message, params object[] ps)
    {
        return LogAsync(LogLevel.Fatal, message, ps);
    }

    internal Task<LogWriteOperation[]> LogAsync(LogLevel logLevel, string message, Exception? ex)
    {
        return LogInternal(logLevel, message, null, ex, false);
    }

    internal Task<LogWriteOperation[]> LogAsync(LogLevel logLevel, string message, params object[] ps)
    {
        return LogInternal(logLevel, message, ps, null, true);
    }

    private Task<LogWriteOperation[]> LogInternal(
        LogLevel level,
        string message,
        object[] ps,
        Exception? ex,
        bool doFormat)
    {
        try
        {
            if (_configuration.IsEnabled == false)
            {
                return EmptyOperations;
            }

            var targets = _configuration.GetTargets(level);
            if (!targets.Any())
            {
                return EmptyOperations;
            }

            // format?
            if (doFormat)
            {
                message = string.Format(message, ps);
            }

            // create an event entry and pass it through...
            var entry = new LogEventInfo(level, Name, message, ex);

            // create a context...
            var context = new LogWriteContext();

            // gather the tasks...
            var writeTasks = from target in targets select target.WriteAsync(context, entry);

            // group...
            var group = Task.WhenAll(writeTasks);
            return group;
        }
        catch (Exception? logEx)
        {
            InternalLogger.Current.Error("Logging operation failed.", logEx);
            return EmptyOperations;
        }
    }
}