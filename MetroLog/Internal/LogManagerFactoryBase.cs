using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Targets;

namespace MetroLog.Internal
{
    public abstract class LogManagerFactoryBase
    {
        private static ILogManager _defaultLogManager;

        private LoggingConfiguration DefaultSettings
        {
            get { return CreateDefaultSettings(); }
        }

        protected ILogManager CreateNew(LoggingConfiguration configuration = null)
        {
            return new LogManager(configuration ?? DefaultSettings);
        }


        protected virtual LoggingConfiguration CreateDefaultSettings()
        {
            // default logging config...
            var configuration = new LoggingConfiguration();
            configuration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new DebugTarget());

            return configuration;
        }

        public static ILogManager DefaultLogManager
        {
            get
            {
                if (_defaultLogManager == null)
                    throw new InvalidOperationException("LogManagerFactory.Initialize() must be called first.");

                return _defaultLogManager;
            }
            private set { _defaultLogManager = value; }
        }

        protected static void SetDefaultLogManager(ILogManager instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            if (_defaultLogManager != null)
                throw new InvalidOperationException("Already Initalized. Cannot call LogManagerFactory.Initialize() multiple times.");

            DefaultLogManager = instance;
        }
    }
}