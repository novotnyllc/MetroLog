namespace MetroLog;

public interface ILogConfigurator
{
    LoggingConfiguration CreateDefaultSettings();

    void OnLogManagerCreated(ILogManager manager);
}