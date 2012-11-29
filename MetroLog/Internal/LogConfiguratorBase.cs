using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            //configuration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new EtwTarget());

            return configuration;
        }

        public virtual void OnLogManagerCreated(ILogManager manager)
        {
            
        }
    }
}

