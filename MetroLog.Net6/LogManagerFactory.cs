using MetroLog.Internal;

namespace MetroLog;

public static class LogManagerFactory
{
    private static readonly ILogConfigurator Configurator = new LogConfigurator();

    private static readonly Lazy<ILogManager> LazyLogManager;

    private static LoggingConfiguration _defaultConfig = Configurator.CreateDefaultSettings();

    static LogManagerFactory()
    {
        LazyLogManager = new Lazy<ILogManager>(() => CreateLogManager(), LazyThreadSafetyMode.ExecutionAndPublication);
    }

    public static LoggingConfiguration DefaultConfiguration
    {
        get => _defaultConfig;
        set
        {
            if (LazyLogManager.IsValueCreated)
            {
                throw new InvalidOperationException(
                    "Must set DefaultConfiguration before any calls to DefaultLogManager");
            }

            _defaultConfig = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public static ILogManager DefaultLogManager => LazyLogManager.Value;

    public static LoggingConfiguration CreateLibraryDefaultSettings()
    {
        return Configurator.CreateDefaultSettings();
    }

    public static ILogManager CreateLogManager(LoggingConfiguration? config = null)
    {
        var cfg = config ?? DefaultConfiguration;
        cfg.Freeze();

        var manager = new LogManager(cfg);

        Configurator.OnLogManagerCreated(manager);

        return manager;
    }
}