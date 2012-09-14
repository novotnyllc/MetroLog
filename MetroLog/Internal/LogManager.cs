using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace MetroLog.Internal
{
    internal class LogManager : ILogManager
    {
        public LoggingConfiguration DefaultConfiguration { get; private set; }

        private readonly Dictionary<string, Logger> _loggers;
        private readonly object _loggersLock = new object();

        public event EventHandler<LoggerEventArgs> LoggerCreated;

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

        public ILogger GetLogger(string name = null, LoggingConfiguration config = null)
        {
            lock (_loggersLock)
            {
                if (!(_loggers.ContainsKey(name)))
                {
                    var logger = new Logger(name, config ?? DefaultConfiguration)
                    {
                        Manager = this
                    };

                    // call...
                    this.OnLoggerCreatedSafe(new LoggerEventArgs(logger));

                    // set...
                    _loggers[name] = logger;
                }
                return _loggers[name];
            }
        }

        private void OnLoggerCreatedSafe(LoggerEventArgs args)
        {
            try
            {
                this.OnLoggerCreated(args);
            }
            catch (Exception ex)
            {
                LogInternal("Failed to handle OnLoggerCreated event.", ex);
            }
        }

        protected virtual void OnLoggerCreated(LoggerEventArgs args)
        {
            if (this.LoggerCreated != null)
                this.LoggerCreated(this, args);
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
