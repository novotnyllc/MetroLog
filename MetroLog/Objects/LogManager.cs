using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MetroLog.Targets;

namespace MetroLog
{
    internal class LogManager : ILogManager
    {
        public LoggingConfiguration DefaultConfiguration { get; private set; }

        private readonly Dictionary<string, Logger> _loggers;
        private readonly object _loggersLock = new object();

        internal const string DateTimeFormat = "o";

        public LogManager(LoggingConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            _loggers = new Dictionary<string, Logger>(StringComparer.OrdinalIgnoreCase);

            DefaultConfiguration = configuration;
        }

        public ILogger GetLogger<T>(LoggingConfiguration config = null)
        {
            return GetLogger(typeof(T).Name, config);
        }

        public ILogger GetLogger(string name, LoggingConfiguration config = null)
        {
            lock (_loggersLock)
            {
                if (!(_loggers.ContainsKey(name)))
                    _loggers[name] = new Logger(name, config ?? DefaultConfiguration);
                return _loggers[name];
            }
        }


        // logs problems with the framework to Debug...
        internal static void LogInternal(string message, Exception ex)
        {
            if(ex != null)
                Debug.WriteLine("{0}|INTERNAL|(null)|{1} --> {2}", GetDateTime().ToString(DateTimeFormat), message, ex);
            else
                Debug.WriteLine("{0}|INTERNAL|(null)|{1}", GetDateTime().ToString(DateTimeFormat), message);
        }

        internal static DateTimeOffset GetDateTime()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}
