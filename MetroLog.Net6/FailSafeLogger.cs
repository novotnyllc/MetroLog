namespace MetroLog;

public class FailSafeLogger : ILogger
{
    public string Name { get; }

    public bool IsTraceEnabled { get; }

    public bool IsDebugEnabled { get; }

    public bool IsInfoEnabled { get; }

    public bool IsWarnEnabled { get; }

    public bool IsErrorEnabled { get; }

    public bool IsFatalEnabled { get; }

    public void Trace(string message, Exception? ex = null)
    {
        WriteLine("Trace", message, ex);
    }

    public void Trace(string message, params object[] ps)
    {
        throw new NotImplementedException();
    }

    public void Debug(string message, Exception? ex = null)
    {
        WriteLine("Debug", message, ex);
    }

    public void Debug(string message, params object[] ps)
    {
        throw new NotImplementedException();
    }

    public void Info(string message, Exception? ex = null)
    {
        WriteLine("Info", message, ex);
    }

    public void Info(string message, params object[] ps)
    {
        throw new NotImplementedException();
    }

    public void Warn(string message, Exception? ex = null)
    {
        WriteLine("Warn", message, ex);
    }

    public void Warn(string message, params object[] ps)
    {
        throw new NotImplementedException();
    }

    public void Error(string message, Exception? ex = null)
    {
        WriteLine("Error", message, ex);
    }

    public void Error(string message, params object[] ps)
    {
        throw new NotImplementedException();
    }

    public void Fatal(string message, Exception? ex = null)
    {
        WriteLine("Fatal", message, ex);
    }

    public void Fatal(string message, params object[] ps)
    {
        throw new NotImplementedException();
    }

    public void Log(LogLevel logLevel, string message, Exception? ex)
    {
        WriteLine(logLevel.ToString(), message, ex);
    }

    public void Log(LogLevel logLevel, string message, params object[] ps)
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel level)
    {
        throw new NotImplementedException();
    }

    private void WriteLine(string tag, string message, Exception? ex = null)
    {
        Console.WriteLine($@"{tag}: {message}");
        if (ex != null)
        {
            Console.WriteLine($@"{tag}: {ex}");
        }
    }
}