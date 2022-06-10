namespace MetroLog;

public class LoggerEventArgs : EventArgs
{
    public LoggerEventArgs(ILogger logger)
    {
        Logger = logger;
    }

    public ILogger Logger { get; }
}