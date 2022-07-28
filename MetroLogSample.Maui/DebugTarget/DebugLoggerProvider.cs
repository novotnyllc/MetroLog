using MetroLog.MicrosoftExtensions;
using MetroLog.Targets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MetroLogSample.Maui.DebugTarget;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddDebugLogger(
        this ILoggingBuilder loggerBuilder,
        Action<LoggerOptions> configure)
    {
        loggerBuilder.Services.AddSingleton<ILoggerProvider, DebugLoggerProvider>();
        loggerBuilder.Services.Configure(configure);
        return loggerBuilder;
    }
}

[ProviderAlias("Debug")]
public class DebugLoggerProvider : LoggerProviderBase
{
    public DebugLoggerProvider(IOptions<LoggerOptions> options)
        : base(new DebugTarget(), options.Value)
    {
    }
}
