using System;
using System.IO;
using System.Threading;
using MetroLog.Internal;

namespace MetroLog
{
    public static class LogManagerFactory
    {
        private static readonly ILogConfigurator _configurator = PlatformAdapter.Resolve<ILogConfigurator>();

        private static LoggingConfiguration _defaultConfig = _configurator.CreateDefaultSettings();

        private static readonly Lazy<ILogManager> _lazyLogManager;

        static LogManagerFactory()
        {
            _lazyLogManager = new Lazy<ILogManager>(() => CreateLogManager(),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }
      
        public static LoggingConfiguration CreateLibraryDefaultSettings()
        {
            return _configurator.CreateDefaultSettings();
        }

        public static LoggingConfiguration CreateLibrarySettings(Stream configFileStream)
        {
            return _configurator.CreateFromXml(configFileStream);
        }

        public static ILogManager CreateLogManager(LoggingConfiguration config = null)
        {
            var cfg = config ?? DefaultConfiguration;
            cfg.Freeze();


            ILogManager manager;
            var managerFactory = PlatformAdapter.Resolve<ILogManagerCreator>(false);
            if (managerFactory != null)
                manager = managerFactory.Create(cfg);
            else
                manager = new LogManagerBase(cfg);

            _configurator.OnLogManagerCreated(manager);

            return manager;
        }

        public static LoggingConfiguration DefaultConfiguration
        {
            get
            {
                return _defaultConfig;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (_lazyLogManager.IsValueCreated)
                    throw new InvalidOperationException("Must set DefaultConfiguration before any calls to DefaultLogManager");

                _defaultConfig = value;
            }
        }

        public static ILogManager DefaultLogManager
        {
            get { return _lazyLogManager.Value; }
        }

    }
}