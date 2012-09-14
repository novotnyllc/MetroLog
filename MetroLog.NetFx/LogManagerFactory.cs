using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Internal;
using MetroLog.Targets;

namespace MetroLog
{
    public class LogManagerFactory : LogManagerFactoryBase
    {
        protected LogManagerFactory()
        {
            
        }

        public static void Initialize(LoggingConfiguration config = null)
        {
            var factory = new LogManagerFactory();

            var instance = factory.CreateNew(new LoggingEnvironment(), config);

            SetDefaultLogManager(instance);
        }

        protected override LoggingConfiguration CreateDefaultSettings()
        {
            var def = base.CreateDefaultSettings();
            def.AddTarget(LogLevel.Trace, LogLevel.Fatal, new TraceTarget());

            return def;
        }

        /// <summary>
        /// Reinitializes the log manager.
        /// </summary>
        /// <param name="config"></param>
        /// <remarks>Only use for testing.</remarks>
        internal static void Reinitialize(LoggingConfiguration config)
        {
            var factory = new LogManagerFactory();

            var instance = factory.CreateNew(new LoggingEnvironment(), config);

            SetDefaultLogManager(instance, true);
        }
    }
}
