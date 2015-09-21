using System;
using System.IO;
using System.Threading;
using CrossPlatformAdapter;
using MetroLog.Internal;

namespace MetroLog
{
    public static class LogManagerFactory
    {
        private static readonly ILogConfigurator configurator = PlatformAdapter.Current.Resolve<ILogConfigurator>();
        private static readonly Lazy<ILogManager> lazyLogManager;
        private static LoggingConfiguration defaultConfig = configurator.CreateDefaultSettings();

        static LogManagerFactory()
        {
            lazyLogManager = new Lazy<ILogManager>(() => CreateLogManager(), LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public static LoggingConfiguration CreateLibraryDefaultSettings()
        {
            return configurator.CreateDefaultSettings();
        }

        public static LoggingConfiguration CreateLibrarySettings(Stream configFileStream)
        {
            return configurator.CreateFromXml(configFileStream);
        }

        public static ILogManager CreateLogManager(LoggingConfiguration config = null)
        {
            var cfg = config ?? DefaultConfiguration;
            cfg.Freeze();
            
            ILogManager manager;
            var logManagerCreator = PlatformAdapter.Current.TryResolve<ILogManagerCreator>();
            if (logManagerCreator != null)
            {
                manager = logManagerCreator.Create(cfg);
            }
            else
            {
                manager = new LogManagerBase(cfg);
            }

            configurator.OnLogManagerCreated(manager);

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
                {
                    throw new ArgumentNullException("value");
                }
                if (lazyLogManager.IsValueCreated)
                {
                    throw new InvalidOperationException("Must set DefaultConfiguration before any calls to DefaultLogManager");
                }

                defaultConfig = value;
            }
        }

        public static ILogManager DefaultLogManager
        {
            get
            {
                return lazyLogManager.Value;
            }
        }
    }
}