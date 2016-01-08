using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Internal;
using MetroLog.Targets;

namespace MetroLog
{
    sealed class LogConfigurator : LogConfiguratorBase
    {


        public override LoggingConfiguration CreateDefaultSettings()
        {
            var def = base.CreateDefaultSettings();
            def.AddTarget(LogLevel.Trace, LogLevel.Fatal, new StreamingFileTarget());

            return def;
        }

    }
}
