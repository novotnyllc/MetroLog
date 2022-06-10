using MetroLog.Targets;

namespace MetroLog.Internal;

internal class LogManager : ILogManager
{
    internal const string DateTimeFormat = "o";

    private readonly Dictionary<string, Logger> _loggers;
    private readonly object _loggersLock = new();

    private readonly List<ILogOperator> _operators;

    public LogManager(LoggingConfiguration configuration)
    {
        _loggers = new Dictionary<string, Logger>(StringComparer.OrdinalIgnoreCase);

        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        _operators = Configuration.GetTargets().OfType<ILogOperator>().ToList();
    }

    public event EventHandler<LoggerEventArgs>? LoggerCreated;

    public LoggingConfiguration Configuration { get; }

    public bool TryGetOperator<TInterface>(out TInterface? @operator) where TInterface : ILogOperator
    {
        foreach (var logOperator in _operators)
        {
            if (logOperator is TInterface specificOperator)
            {
                @operator = specificOperator;
                return true;
            }
        }

        @operator = default;
        return false;
    }

    /// <summary>
    ///     Gets the logger for the given object of type <c>T</c>.
    /// </summary>
    /// <typeparam name="T">The type of object.</typeparam>
    /// <param name="config">An optional configuration value.</param>
    /// <returns>The requested logger.</returns>
    public ILogger GetLogger<T>(LoggingConfiguration? config = null)
    {
        return GetLogger(typeof(T), config);
    }

    /// <summary>
    ///     Gets the logger for the given type.
    /// </summary>
    /// <param name="type">The type of object.</param>
    /// <param name="config">An optional configuration value.</param>
    /// <returns>The requested logger.</returns>
    public ILogger GetLogger(Type type, LoggingConfiguration? config = null)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        return GetLogger(type.Name, config);
    }

    /// <summary>
    ///     Gets the logger with the given name.
    /// </summary>
    /// <param name="name">The name of the logger.</param>
    /// <param name="config">An optional configuration value.</param>
    /// <returns>The requested logger.</returns>
    public ILogger GetLogger(string name, LoggingConfiguration? config = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        lock (_loggersLock)
        {
            if (!_loggers.ContainsKey(name))
            {
                var logger = new Logger(name, config ?? Configuration);
                InternalLogger.Current.Info("Created Logger '{0}'", name);

                // call...
                OnLoggerCreatedSafe(new LoggerEventArgs(logger));

                // set...
                _loggers[name] = logger;
            }

            return _loggers[name];
        }
    }

    internal static DateTimeOffset GetDateTime()
    {
        return DateTimeOffset.UtcNow;
    }

    private void OnLoggerCreatedSafe(LoggerEventArgs args)
    {
        try
        {
            OnLoggerCreated(args);
        }
        catch (Exception? ex)
        {
            InternalLogger.Current.Error("Failed to handle OnLoggerCreated event.", ex);
        }
    }

    private void OnLoggerCreated(LoggerEventArgs args)
    {
        LoggerCreated?.Invoke(this, args);
    }
}
