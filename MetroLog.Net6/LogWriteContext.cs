namespace MetroLog;

public class LogWriteContext
{
    private static readonly ILoggingEnvironment _environment;

    static LogWriteContext()
    {
        _environment = new LoggingEnvironment();
    }

    public ILoggingEnvironment Environment => _environment;
}