using System.IO;

using CrossPlatformAdapter;

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
            var assemblyService = PlatformAdapter.Current.Resolve<IAssemblyService>();
            XmlConfigurator xmlConfigurator = new XmlConfigurator(assemblyService);
            return xmlConfigurator.Configure(configFileStream);
        }

        public virtual void OnLogManagerCreated(ILogManager manager)
        {
            
        }
    }
}

