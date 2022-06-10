namespace MetroLog.Targets;

public class LogReadQuery
{
    public LogReadQuery()
    {
        IsTraceEnabled = false;
        IsDebugEnabled = false;
        IsInfoEnabled = true;
        IsWarnEnabled = true;
        IsErrorEnabled = true;
        IsFatalEnabled = true;

        Top = 1000;

        FromDateTimeUtc = DateTime.UtcNow.AddDays(-7);
    }

    public bool IsTraceEnabled { get; set; }

    public bool IsDebugEnabled { get; set; }

    public bool IsInfoEnabled { get; set; }

    public bool IsWarnEnabled { get; set; }

    public bool IsErrorEnabled { get; set; }

    public bool IsFatalEnabled { get; set; }

    /// <summary>
    ///     Gets or sets the number of items to read.
    /// </summary>
    /// <remarks>By default this is set to <c>1000</c>. Set to <c>0</c> to remove any limit.</remarks>
    public int Top { get; }

    /// <summary>
    ///     Gets or sets the earliest date/time to read.
    /// </summary>
    /// <remarks>
    ///     By default this is <c>DateTime.UtcNow.AddDays(-7)</c>. Set to <c>DateTime.MinValue</c> to remove this
    ///     constraint.
    /// </remarks>
    public DateTime FromDateTimeUtc { get; }

    public void SetLevels(LogLevel from, LogLevel to)
    {
        IsTraceEnabled = LogLevel.Trace >= from && LogLevel.Trace <= to;
        IsDebugEnabled = LogLevel.Debug >= from && LogLevel.Debug <= to;
        IsInfoEnabled = LogLevel.Info >= from && LogLevel.Info <= to;
        IsWarnEnabled = LogLevel.Warn >= from && LogLevel.Warn <= to;
        IsErrorEnabled = LogLevel.Error >= from && LogLevel.Error <= to;
        IsFatalEnabled = LogLevel.Fatal >= from && LogLevel.Fatal <= to;
    }
}