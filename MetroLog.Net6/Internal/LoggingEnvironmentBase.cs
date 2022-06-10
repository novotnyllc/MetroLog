using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace MetroLog.Internal;

public abstract class LoggingEnvironmentBase : ILoggingEnvironment
{
    protected LoggingEnvironmentBase(string fxProfile)
    {
        // common...
        SessionId = Guid.NewGuid();
        FxProfile = fxProfile;
        IsDebugging = Debugger.IsAttached;
        MetroLogVersion = typeof(ILogger).GetTypeInfo().Assembly.GetName().Version!;
    }

    public string FxProfile { get; }

    public bool IsDebugging { get; }

    public Version MetroLogVersion { get; }

    public Guid SessionId { get; }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}