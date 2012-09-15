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
        private static readonly ILogConfigurator _configurator = PlatformAdapter.Resolve<ILogConfigurator>();

        private static LoggingConfiguration _defaultConfig = _configurator.CreateDefaultSettings();

        private static readonly Lazy<ILogManager> _lazyLogManager;

        static LogManagerFactory()
        {
            _lazyLogManager = new Lazy<ILogManager>(() =>
                {
                    var config = DefaultConfiguration;
                    config.Freeze();

                    var manager = new LogManager(config);
                    _configurator.OnLogManagerCreated(manager);
                    return manager;
                },
                LazyThreadSafetyMode.ExecutionAndPublication);
        }
      
        public static LoggingConfiguration CreateLibraryDefaultSettings()
        {
            return _configurator.CreateDefaultSettings();
        }

        public static ILogManager CreateLogManager(LoggingConfiguration config = null)
        {
            var cfg = config ?? DefaultConfiguration;
            cfg.Freeze();

            return new LogManager(cfg);
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