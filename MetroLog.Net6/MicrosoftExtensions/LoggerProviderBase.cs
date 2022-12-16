using MetroLog.Layouts;
using MetroLog.Targets;
using Microsoft.Extensions.Logging;

namespace MetroLog.MicrosoftExtensions;

public class LoggerOptions
{
    /// <summary>
    /// If true, the logger name won't embed the namespace, just the class name.
    /// </summary>
    public virtual bool ShortCategory { get; set; } = true;

    public virtual Microsoft.Extensions.Logging.LogLevel? MinLevel { get; set; }

    public virtual Microsoft.Extensions.Logging.LogLevel? MaxLevel { get; set; }

    public virtual Layout? Layout { get; set; }
}

public abstract class LoggerProviderBase : ILoggerProvider
{
    private readonly bool _shortCategory;

    protected LoggerProviderBase(Target target, LoggerOptions options)
    {
        var configuration = new LoggingConfiguration();
        configuration.AddTarget(
            options.MinLevel?.ToMetroLogLevel() ?? LogLevel.Info,
            options.MaxLevel?.ToMetroLogLevel() ?? LogLevel.Fatal,
            target);

        _shortCategory = options.ShortCategory;

        LogManager = LogManagerFactory.CreateLogManager(configuration);
    }

    protected ILogManager LogManager { get; }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
    {
        if (_shortCategory)
        {
            int lastIndex = categoryName.LastIndexOf('.');
            if (lastIndex > -1)
            {
                categoryName = categoryName.Substring(lastIndex + 1);
            }
        }

        InternalLogger.Current.Debug($"CreateLogger {categoryName}");
        return new MicrosoftLoggerWrapper(LogManager.GetLogger(categoryName));
    }
}