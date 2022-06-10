using MetroLog.Operators;

namespace MetroLog.Internal;

internal abstract class LogConfiguratorBase : ILogConfigurator
{
    public virtual LoggingConfiguration CreateDefaultSettings()
    {
        return LoggingConfiguration.GetDefaultDebugConfiguration();
    }

    public abstract void OnLogManagerCreated(ILogManager manager);
}
