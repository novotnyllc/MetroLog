using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Targets;

namespace MetroLog.Internal
{
    partial class LogManager : ILogManager
    {
        public LoggingConfiguration DefaultConfiguration { get; private set; }

        readonly Dictionary<string, Logger> _loggers;
        readonly object _loggersLock = new object();

        public event EventHandler<LoggerEventArgs> LoggerCreated;
        
        public Task<Stream> GetCompressedLogs()
        {
            // get the first file target if there is one

            var fsb = DefaultConfiguration.GetTargets().OfType<FileTargetBase>().FirstOrDefault();

            if (fsb != null)
            {
                return fsb.GetCompressedLogs();
            }

            return Task.FromResult<Stream>(null);
        }

        internal const string DateTimeFormat = "o";

        public LogManager(LoggingConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _loggers = new Dictionary<string, Logger>(StringComparer.OrdinalIgnoreCase);

            DefaultConfiguration = configuration;
        }

        /// <summary>
        /// Gets the logger for the given object of type <c>T</c>.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="config">An optional configuration value.</param>
        /// <returns>The requested logger.</returns>
        public ILogger GetLogger<T>(LoggingConfiguration config = null)
        {
            return GetLogger(typeof(T), config);
        }

        /// <summary>
        /// Gets the logger for the given type.
        /// </summary>
        /// <param name="type">The type of object.</param>
        /// <param name="config">An optional configuration value.</param>
        /// <returns>The requested logger.</returns>
        public ILogger GetLogger(Type type, LoggingConfiguration config = null)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return GetLogger(type.Name, config);
        }

        /// <summary>
        /// Gets the logger with the given name.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        /// <param name="config">An optional configuration value.</param>
        /// <returns>The requested logger.</returns>
        public ILogger GetLogger(string name, LoggingConfiguration config = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            lock (_loggersLock)
            {
                if (!(_loggers.ContainsKey(name)))
                {
                    var logger = new Logger(name, config ?? DefaultConfiguration);
                    InternalLogger.Current.Info("Created Logger '{0}'", name);

                    // call...
                    OnLoggerCreatedSafe(new LoggerEventArgs(logger));

                    // set...
                    _loggers[name] = logger;
                }
                return _loggers[name];
            }
        }

        void OnLoggerCreatedSafe(LoggerEventArgs args)
        {
            try
            {
                OnLoggerCreated(args);
            }
            catch (Exception ex)
            {
                InternalLogger.Current.Error("Failed to handle OnLoggerCreated event.", ex);
            }
        }

        void OnLoggerCreated(LoggerEventArgs args)
        {
            var evt = LoggerCreated;
            if (evt != null)
                evt(this, args);
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
    }
}
