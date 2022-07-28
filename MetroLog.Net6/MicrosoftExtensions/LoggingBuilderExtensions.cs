using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MetroLog.MicrosoftExtensions;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddStreamingFileLogger(
        this ILoggingBuilder loggerBuilder,
        Action<StreamingFileLoggerOptions> configure)
    {
        loggerBuilder.Services.AddSingleton<ILoggerProvider, StreamingFileLoggerProvider>();
        loggerBuilder.Services.Configure(configure);
        return loggerBuilder;
    }

    public static ILoggingBuilder AddInMemoryLogger(
        this ILoggingBuilder loggerBuilder,
        Action<InMemoryLoggerOptions> configure)
    {
        loggerBuilder.Services.AddSingleton<ILoggerProvider, InMemoryLoggerProvider>();
        loggerBuilder.Services.Configure(configure);
        return loggerBuilder;
    }

    public static ILoggingBuilder AddConsoleLogger(
        this ILoggingBuilder loggerBuilder,
        Action<LoggerOptions> configure)
    {
        loggerBuilder.Services.AddSingleton<ILoggerProvider, ConsoleLoggerProvider>();
        loggerBuilder.Services.Configure(configure);
        return loggerBuilder;
    }

    public static ILoggingBuilder AddTraceLogger(
        this ILoggingBuilder loggerBuilder,
        Action<LoggerOptions> configure)
    {
        loggerBuilder.Services.AddSingleton<ILoggerProvider, TraceLoggerProvider>();
        loggerBuilder.Services.Configure(configure);
        return loggerBuilder;
    }
}
