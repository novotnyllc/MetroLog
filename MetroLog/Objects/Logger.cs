using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    internal class Logger : ILogger
    {
        public string Name { get; private set; }
        private readonly LoggingConfiguration _configuration;

        internal Logger(string name, LoggingConfiguration config)
        {
            Name = name;

            // add a target...
            _configuration = config;
        }


        internal Task<LogWriteOperation[]> TraceAsync(string message, Exception ex = null)
        {
            return LogAsync(LogLevel.Trace, message, ex);
        }

        internal Task<LogWriteOperation[]> TraceAsync(string message, params object[] ps)
        {
            return LogAsync(LogLevel.Trace, message, ps);
        }

        internal Task<LogWriteOperation[]> DebugAsync(string message, Exception ex = null)
        {
            return LogAsync(LogLevel.Debug, message, ex);
        }

        internal Task<LogWriteOperation[]> DebugAsync(string message, params object[] ps)
        {
            return LogAsync(LogLevel.Debug, message, ps);
        }

        internal Task<LogWriteOperation[]> InfoAsync(string message, Exception ex = null)
        {
            return LogAsync(LogLevel.Info, message, ex);
        }

        internal Task<LogWriteOperation[]> InfoAsync(string message, params object[] ps)
        {
            return LogAsync(LogLevel.Info, message, ps);
        }

        internal Task<LogWriteOperation[]> WarnAsync(string message, Exception ex = null)
        {
            return LogAsync(LogLevel.Warn, message, ex);
        }

        internal Task<LogWriteOperation[]> WarnAsync(string message, params object[] ps)
        {
            return LogAsync(LogLevel.Warn, message, ps);
        }

        internal Task<LogWriteOperation[]> ErrorAsync(string message, Exception ex = null)
        {
            return LogAsync(LogLevel.Error, message, ex);
        }

        internal Task<LogWriteOperation[]> ErrorAsync(string message, params object[] ps)
        {
            return LogAsync(LogLevel.Error, message, ps);
        }

        internal Task<LogWriteOperation[]> FatalAsync(string message, Exception ex = null)
        {
            return LogAsync(LogLevel.Fatal, message, ex);
        }

        internal Task<LogWriteOperation[]> FatalAsync(string message, params object[] ps)
        {
            return LogAsync(LogLevel.Fatal, message, ps);
        }

        internal Task<LogWriteOperation[]> LogAsync(LogLevel logLevel, string message, Exception ex)
        {
            return LogInternal(logLevel, message, null, ex, false);
        }

        internal Task<LogWriteOperation[]> LogAsync(LogLevel logLevel, string message, params object[] ps)
        {
            return LogInternal(logLevel, message, ps, null, true);
        }

        private Task<LogWriteOperation[]> LogInternal(LogLevel level, string message, object[] ps, Exception ex, bool doFormat)
        {
            try
            {
                var targets = _configuration.GetTargets(level);
                if (!(targets.Any()))
                    return Task.FromResult(new LogWriteOperation[] { });

                // format?
                if (doFormat)
                    message = string.Format(message, ps);

                // create an event entry and pass it through...
                var entry = new LogEventInfo(level, Name, message, ex);

                var writeTasks = from target in targets
                                 select target.WriteAsync(entry);

                // group...
                var group = Task.WhenAll(writeTasks);
                return group;
            }
            catch (Exception logEx)
            {
                LogManager.LogInternal("Logging operation failed.", logEx);
                return Task.FromResult(new LogWriteOperation[] {});
            }
        }

        public bool IsTraceEnabled
        {
            get
            {
                return IsEnabled(LogLevel.Trace);
            }
        }

        public bool IsDebugEnabled
        {
            get
            {
                return IsEnabled(LogLevel.Debug);
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                return IsEnabled(LogLevel.Info);
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                return IsEnabled(LogLevel.Warn);
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                return IsEnabled(LogLevel.Error);
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
                return IsEnabled(LogLevel.Fatal);
            }
        }

        public void Trace(string message, Exception ex = null)
        {
            TraceAsync(message, ex);
        }

        public void Trace(string message, params object[] ps)
        {
            TraceAsync(message, ps);
        }

        public void Debug(string message, Exception ex = null)
        {
            DebugAsync(message, ex);
        }

        public void Debug(string message, params object[] ps)
        {
            DebugAsync(message, ps);
        }

        public void Info(string message, Exception ex = null)
        {
            InfoAsync(message, ex);
        }

        public void Info(string message, params object[] ps)
        {
            InfoAsync(message, ps);
        }

        public void Warn(string message, Exception ex = null)
        {
            WarnAsync(message, ex);
        }

        public void Warn(string message, params object[] ps)
        {
            WarnAsync(message, ps);
        }

        public void Error(string message, Exception ex = null)
        {
            ErrorAsync(message, ex);
        }

        public void Error(string message, params object[] ps)
        {
            ErrorAsync(message, ps);
        }

        public void Fatal(string message, Exception ex = null)
        {
            FatalAsync(message, ex);
        }

        public void Fatal(string message, params object[] ps)
        {
            FatalAsync(message, ps);
        }

        public void Log(LogLevel logLevel, string message, Exception ex)
        {
            LogAsync(logLevel, message, ex);
        }

        public void Log(LogLevel logLevel, string message, params object[] ps)
        {
            LogAsync(logLevel, message, ps);
        }

        public bool IsEnabled(LogLevel level)
        {
            return _configuration.GetTargets(level).Any();
        }
    }
}
