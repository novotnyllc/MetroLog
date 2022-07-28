using MetroLog.Targets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MetroLog.MicrosoftExtensions;


[ProviderAlias("Trace")]
public class TraceLoggerProvider : LoggerProviderBase
{
    public TraceLoggerProvider(IOptions<LoggerOptions> options)
        : base(new TraceTarget(), options.Value)
    {
    }
}
