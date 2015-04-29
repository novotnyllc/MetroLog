using System.IO;

using MetroLog.Config;
using MetroLog.Targets;

namespace MetroLog.Internal
{
    public class LogConfiguratorBase : ILogConfigurator
    {
        public virtual LoggingConfiguration CreateDefaultSettings()
        {
            // default logging config...
            var configuration = new LoggingConfiguration();
            configuration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new DebugTarget());

            return configuration;
        }

        public LoggingConfiguration CreateFromXml(Stream configFileStream)
        {
            XmlConfigurator xmlConfigurator = new XmlConfigurator();
            return xmlConfigurator.Configure(configFileStream);
        }

        public virtual void OnLogManagerCreated(ILogManager manager)
        {
            
        }
    }
}

