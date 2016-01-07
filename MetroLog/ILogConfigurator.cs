using System.IO;

namespace MetroLog
{
    public interface ILogConfigurator
    {
        LoggingConfiguration CreateDefaultSettings();

        LoggingConfiguration CreateFromXml(Stream configFileStream);

        void OnLogManagerCreated(ILogManager manager);
    }
}
