using MetroLog.Layouts;
using MetroLog.Targets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MetroLog.MicrosoftExtensions;

public class StreamingFileLoggerOptions : LoggerOptions
{
    public virtual string? FolderPath { get; set; }

    public virtual int? RetainDays { get; set; }

    public virtual FileNamingParameters? FileNamingParameters { get; set; }
}

[ProviderAlias("StreamingFile")]
public class StreamingFileLoggerProvider : LoggerProviderBase
{
    public StreamingFileLoggerProvider(IOptions<StreamingFileLoggerOptions> options)
        : base(
            new StreamingFileTarget(
                options.Value.Layout ?? new SingleLineLayout(),
                logsDirectoryPath: options.Value.FolderPath,
                retainDays: options.Value.RetainDays ?? 30,
                fileNamingParameters: options.Value.FileNamingParameters),
            options.Value)
    {
    }
}