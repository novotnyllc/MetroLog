using System.Text;
using System.Text.RegularExpressions;

namespace MetroLog.Targets;

/// <summary>
///     Defines a class that allows the user to configure file naming.
/// </summary>
public class FileNamingParameters
{
    public FileNamingParameters()
    {
        IncludeLevel = false;
        IncludeTimestamp = FileTimestampMode.Date;
        IncludeLogger = false;
        IncludeSession = true;
        IncludeSequence = false;
        CreationMode = FileCreationMode.AppendIfExisting;
    }

    public bool IncludeLevel { get; set; }

    public FileTimestampMode IncludeTimestamp { get; set; }

    public bool IncludeLogger { get; set; }

    public bool IncludeSession { get; set; }

    public bool IncludeSequence { get; set; }

    public FileCreationMode CreationMode { get; set; }

    public string GetFilename(LogWriteContext context, LogEventInfo entry)
    {
        var builder = new StringBuilder();
        builder.Append("Log");
        if (IncludeLevel)
        {
            builder.Append(" - ");
            builder.Append(entry.Level.ToString().ToUpper());
        }

        if (IncludeLogger)
        {
            builder.Append(" - ");
            builder.Append(entry.Logger);
        }

        if (IncludeTimestamp != FileTimestampMode.None)
        {
            var date = ((int)IncludeTimestamp & (int)FileTimestampMode.Date) != 0;
            if (date)
            {
                builder.Append(" - ");
                builder.Append(entry.TimeStamp.ToString("yyyyMMdd"));
            }

            var time = ((int)IncludeTimestamp & (int)FileTimestampMode.Time) != 0;
            if (time)
            {
                builder.Append(date ? " " : " - ");
                builder.Append(entry.TimeStamp.ToString("HHmmss"));
            }
        }

        if (IncludeSession)
        {
            builder.Append(" - ");
            builder.Append(context.Environment.SessionId);
        }

        if (IncludeSequence)
        {
            builder.Append(" - ");
            builder.Append(entry.SequenceId);
        }

        // return...
        builder.Append(".log");
        return builder.ToString();
    }

    public Regex GetRegex()
    {
        var builder = new StringBuilder();
        builder.Append("^Log");

        // stuff...
        if (IncludeLevel)
        {
            builder.Append(@"\s*-\s*");
            builder.Append(@"\w+");
        }

        if (IncludeLogger)
        {
            builder.Append(@"\s*-\s*");
            builder.Append(@"[\w\s]+");
        }

        if (IncludeTimestamp != FileTimestampMode.None)
        {
            var date = ((int)IncludeTimestamp & (int)FileTimestampMode.Date) != 0;
            if (date)
            {
                builder.Append(@"\s*-\s*");
                builder.Append("[0-9]{8}");
            }

            var time = ((int)IncludeTimestamp & (int)FileTimestampMode.Time) != 0;
            if (time)
            {
                builder.Append(date ? @"\s+" : @"\s*-\s*");
                builder.Append("[0-9]{6}");
            }
        }

        if (IncludeSession)
        {
            builder.Append(@"\s*-\s*");
            builder.Append(@"[a-fA-F0-9\-]+");
        }

        if (IncludeSequence)
        {
            builder.Append(@"\s*-\s*");
            builder.Append("[0-9]+");
        }

        // log...
        builder.Append(".log$");

        // go...
        var regex = new Regex(builder.ToString(), RegexOptions.Singleline | RegexOptions.IgnoreCase);
        return regex;
    }
}