using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using MetroLog.Targets;

namespace MetroLog.Internal
{
    public class LogManagerBase : ILogManager
    {
        public LoggingConfiguration DefaultConfiguration { get; private set; }

        internal const string DateTimeFormat = "o";

        private readonly Dictionary<string, Logger> loggers;
        private readonly object loggersLock = new object();

        public event EventHandler<LoggerEventArgs> LoggerCreated;

        public Task<Stream> GetCompressedLogs()
        {
            // get the first file target if there is one
            var fsb = this.DefaultConfiguration.GetTargets().OfType<FileTargetBase>().FirstOrDefault();
            if (fsb != null)
            {
                return fsb.GetCompressedLogs();
            }

            return Task.FromResult<Stream>(null);
        }

        public LogManagerBase(LoggingConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            this.loggers = new Dictionary<string, Logger>(StringComparer.OrdinalIgnoreCase);
            this.DefaultConfiguration = configuration;
        }

        /// <summary>
        ///     Gets the logger for the given object of type <c>T</c>.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="config">An optional configuration value.</param>
        /// <returns>The requested logger.</returns>
        public ILogger GetLogger<T>(LoggingConfiguration config = null)
        {
            return this.GetLogger(typeof(T), config);
        }

        /// <summary>
        ///     Gets the logger for the given type.
        /// </summary>
        /// <param name="type">The type of object.</param>
        /// <param name="config">An optional configuration value.</param>
        /// <returns>The requested logger.</returns>
        public ILogger GetLogger(Type type, LoggingConfiguration config = null)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return this.GetLogger(type.Name, config);
        }

        /// <summary>
        ///     Gets the logger with the given name.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        /// <param name="config">An optional configuration value.</param>
        /// <returns>The requested logger.</returns>
        public ILogger GetLogger(string name, LoggingConfiguration config = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            lock (this.loggersLock)
            {
                if (!(this.loggers.ContainsKey(name)))
                {
                    var configuration = config ?? this.DefaultConfiguration;
                    var logger = new Logger(name, configuration);
                    InternalLogger.Current.Info("Created Logger '{0}'", name);

                    this.OnLoggerCreatedSafe(new LoggerEventArgs(logger));

                    this.loggers[name] = logger;
                }
                return this.loggers[name];
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
                InternalLogger.Current.Error("Failed to handle OnLoggerCreated event.", ex);
            }
        }

        private void OnLoggerCreated(LoggerEventArgs args)
        {
            var evt = this.LoggerCreated;
            if (evt != null)
            {
                evt(this, args);
            }
        }

        internal static DateTimeOffset GetDateTime()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}