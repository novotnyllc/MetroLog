namespace MetroLog;

public static class LoggerFactory
{
    private static ILogManager? _logManager;

    public static void Initialize(LoggingConfiguration configuration)
    {
        _logManager = LogManagerFactory.CreateLogManager(configuration);
    }

    public static void InitializeWithDebugDefault()
    {
        _logManager = LogManagerFactory.CreateLogManager(LoggingConfiguration.GetDefaultDebugConfiguration());
    }

    public static void InitializeWithReleaseDefault()
    {
        _logManager = LogManagerFactory.CreateLogManager(LoggingConfiguration.GetDefaultReleaseConfiguration());
    }

    public static ILogManager GetLogManager()
    {
        if (_logManager == null)
        {
            throw new InvalidOperationException("LogFactory must be Initialized before creating any logger");
        }

        return _logManager;
    }

    public static ILogger GetLogger(string loggerName)
    {
        if (_logManager == null)
        {
            throw new InvalidOperationException("LogFactory must be Initialized before creating any logger");
        }

        return _logManager.GetLogger(loggerName);
    }

    /// <summary>
    ///     This method never fails and return a FailSafe console logger if the log manager hasn't been initialized.
    /// </summary>
    public static ILogger TryGetLogger(string loggerName)
    {
        if (_logManager == null)
        {
            return new FailSafeLogger();
        }

        return _logManager.GetLogger(loggerName);
    }
}
