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
        public ILoggingEnvironment LoggingEnvironment { get; private set; }

        private readonly Dictionary<string, Logger> _loggers;
        private readonly object _loggersLock = new object();

        public event EventHandler<ILoggerEventArgs> LoggerCreated;
        public event EventHandler CacheReset;

        internal const string DateTimeFormat = "o";

        public LogManager(ILoggingEnvironment environment, LoggingConfiguration configuration)
        {
            if (environment == null)
                throw new ArgumentNullException("environment");
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            _loggers = new Dictionary<string, Logger>(StringComparer.OrdinalIgnoreCase);

            LoggingEnvironment = environment;
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
                    this.OnLoggerCreatedSafe(new ILoggerEventArgs(logger));

                    // set...
                    _loggers[name] = logger;
                }
                return _loggers[name];
            }
        }

        private void OnLoggerCreatedSafe(ILoggerEventArgs args)
        {
            try
            {
                this.OnLoggerCreated(args);
            }
            catch (Exception ex)
            {
                InternalLogger.Current.Error("Failed to handle OnLoggerCreated event.", ex);
            }
        }

        protected virtual void OnLoggerCreated(ILoggerEventArgs args)
        {
            if (this.LoggerCreated != null)
                this.LoggerCreated(this, args);
        }

        // logs problems with the framework to Debug...
        // mbr - 2012-09-14 - moves to InternalLogger...
        //internal static void LogInternal(string message, Exception ex)
        //{
        //    if(ex != null)
        //        Debug.WriteLine("{0}|INTERNAL|(null)|{1} --> {2}", GetDateTime().ToString(DateTimeFormat), message, ex);
        //    else
        //        Debug.WriteLine("{0}|INTERNAL|(null)|{1}", GetDateTime().ToString(DateTimeFormat), message);
        //}

        internal static DateTimeOffset GetDateTime()
        {
            return DateTimeOffset.UtcNow;
        }

        public LogWriteContext GetWriteContext()
        {
            return new LogWriteContext(this);
        }

        /// <summary>
        /// Resets the internal cache of loggers.
        /// </summary>
        /// <remarks>
        /// Not for general use. Used for testing the framework.
        /// </remarks>
        public void ResetCache()
        {
            lock (_loggersLock)
                _loggers.Clear();

            // call...
            this.OnCacheReset();
        }

        protected virtual void OnCacheReset()
        {
            if (this.CacheReset != null)
                this.CacheReset(this, EventArgs.Empty);
        }
    }
}
