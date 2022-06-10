using MetroLog.Targets;

namespace MetroLog;

public interface ILogManager
{
    event EventHandler<LoggerEventArgs>? LoggerCreated;

    LoggingConfiguration Configuration { get; }

    ILogger GetLogger<T>(LoggingConfiguration? config = null);

    ILogger GetLogger(Type type, LoggingConfiguration? config = null);

    ILogger GetLogger(string name, LoggingConfiguration? config = null);

    bool TryGetOperator<TInterface>(out TInterface? @operator) where TInterface : ILogOperator;
}

/// <summary>
///     Marker interface for enabling the Log() mixin
/// </summary>
public interface ICanLog
{
}

public static class LogManagerMixins
{
    public static ILogger Log<T>(this T @this, LoggingConfiguration? config = null) where T : ICanLog
    {
        return LogManagerFactory.DefaultLogManager.GetLogger<T>(config);
    }
}
