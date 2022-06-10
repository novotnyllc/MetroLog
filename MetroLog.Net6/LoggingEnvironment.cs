using System.Reflection;
using MetroLog.Internal;

namespace MetroLog;

/// <summary>
///     Holds the logging environment for .NET apps.
/// </summary>
public class LoggingEnvironment : LoggingEnvironmentBase
{
    private static readonly string? Version;
    private static readonly string? Product;

    static LoggingEnvironment()
    {
        var assm = typeof(AssemblyFileVersionAttribute).GetTypeInfo().Assembly;

        var ver = assm.GetCustomAttribute<AssemblyFileVersionAttribute>();

        Version = ver?.Version;

        var prod = assm.GetCustomAttribute<AssemblyProductAttribute>();
        Product = prod?.Product;
    }

    public LoggingEnvironment()
        : base($"{Product} - {Version}")
    {
        MachineName = Environment.MachineName;
    }

    public string MachineName { get; }
}