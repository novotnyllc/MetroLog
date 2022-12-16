using Microsoft.Extensions.Logging;

namespace MetroLog.MicrosoftExtensions;

public class MicrosoftLoggerWrapper : Microsoft.Extensions.Logging.ILogger
{
    private readonly ILogger _metroLogger;

    public MicrosoftLoggerWrapper(ILogger metroLogger)
    {
        _metroLogger = metroLogger;
    }

    public void Log<TState>(
        Microsoft.Extensions.Logging.LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        _metroLogger.Log(logLevel.ToMetroLogLevel(), formatter(state, exception), exception);
    }

    public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
    {
        return _metroLogger.IsEnabled(logLevel.ToMetroLogLevel());
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        // BeginScope is not supported on MetroLog
        return null;
    }
}

public static class LogLevelExtensions
{
    public static LogLevel ToMetroLogLevel(this Microsoft.Extensions.Logging.LogLevel logLevel)
    {
        return logLevel switch
        {
            Microsoft.Extensions.Logging.LogLevel.Trace => LogLevel.Trace,
            Microsoft.Extensions.Logging.LogLevel.Debug => LogLevel.Debug,
            Microsoft.Extensions.Logging.LogLevel.Information => LogLevel.Info,
            Microsoft.Extensions.Logging.LogLevel.Warning => LogLevel.Warn,
            Microsoft.Extensions.Logging.LogLevel.Error => LogLevel.Error,
            Microsoft.Extensions.Logging.LogLevel.Critical => LogLevel.Fatal,
            _ => throw new NotSupportedException("This log level is not supported on MetroLog"),
        };
    }
}
