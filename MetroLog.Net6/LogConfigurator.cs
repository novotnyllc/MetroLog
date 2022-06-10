using MetroLog.Internal;
using MetroLog.Operators;

namespace MetroLog;

internal sealed class LogConfigurator : LogConfiguratorBase
{
    public override LoggingConfiguration CreateDefaultSettings()
    {
        return LoggingConfiguration.GetDefaultDebugConfiguration();
    }

    public override void OnLogManagerCreated(ILogManager manager)
    {
        InternalLogger.Current.Debug("New log manager created");

        LogOperatorRetriever.AddManager(manager);
    }
}
