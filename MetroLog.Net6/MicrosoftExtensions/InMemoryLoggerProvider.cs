using MetroLog.Layouts;
using MetroLog.Targets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MetroLog.MicrosoftExtensions;

public class InMemoryLoggerOptions : LoggerOptions
{
    public virtual int? MaxLines { get; set; }
}

[ProviderAlias("InMemory")]
public class InMemoryLoggerProvider : LoggerProviderBase
{
    public InMemoryLoggerProvider(IOptions<InMemoryLoggerOptions> options)
        : base(new MemoryTarget(options.Value.MaxLines ?? 1024, options.Value.Layout ?? new SingleLineLayout()), options.Value)
    {
    }
}