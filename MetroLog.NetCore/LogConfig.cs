using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Targets;

namespace MetroLog
{
    public static class LogConfig
    {
        public static void InitializeDefault()
        {
            var configuration = new LoggingConfiguration();
            configuration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new DebugTarget());
            configuration.AddTarget(LogLevel.Error, LogLevel.Fatal, new FileSnapshotTarget());

            LogManager.Reset(configuration);
        }
    }
}
