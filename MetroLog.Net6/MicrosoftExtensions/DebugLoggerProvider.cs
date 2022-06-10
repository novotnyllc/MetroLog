using MetroLog.Targets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MetroLog.MicrosoftExtensions;


[ProviderAlias("Debug")]
public class DebugLoggerProvider : LoggerProviderBase
{
    public DebugLoggerProvider(IOptions<LoggerOptions> options)
        : base(new DebugTarget(), options.Value)
    {
    }
}
