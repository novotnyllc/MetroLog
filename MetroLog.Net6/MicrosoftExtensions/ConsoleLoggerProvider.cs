using MetroLog.Layouts;
using MetroLog.Targets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MetroLog.MicrosoftExtensions;

[ProviderAlias("Console")]
public class ConsoleLoggerProvider : LoggerProviderBase
{
    public ConsoleLoggerProvider(IOptions<LoggerOptions> options)
        : base(new ConsoleTarget(options.Value.Layout ?? new SingleLineLayout()), options.Value)
    {
    }
}