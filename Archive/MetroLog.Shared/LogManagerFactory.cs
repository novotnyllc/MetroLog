using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetroLog.Internal;

namespace MetroLog
{
    public static class LogManagerFactory
    {
        static readonly ILogConfigurator Configurator = new LogConfigurator();

        static LoggingConfiguration defaultConfig = Configurator.CreateDefaultSettings();

        static readonly Lazy<ILogManager> LazyLogManager;

        static LogManagerFactory()
        {
            LazyLogManager = new Lazy<ILogManager>(() => CreateLogManager(),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }
      
        public static LoggingConfiguration CreateLibraryDefaultSettings()
        {
            return Configurator.CreateDefaultSettings();
        }

        public static ILogManager CreateLogManager(LoggingConfiguration config = null)
        {
            var cfg = config ?? DefaultConfiguration;
            cfg.Freeze();

            var manager = new LogManager(cfg);

            Configurator.OnLogManagerCreated(manager);

            return manager;
        }

        public static LoggingConfiguration DefaultConfiguration
        {
            get
            {
                return defaultConfig;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                if (LazyLogManager.IsValueCreated)
                    throw new InvalidOperationException("Must set DefaultConfiguration before any calls to DefaultLogManager");

                defaultConfig = value;
            }
        }

        public static ILogManager DefaultLogManager => LazyLogManager.Value; 
    }
}
