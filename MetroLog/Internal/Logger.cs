using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MetroLog.Targets;

namespace MetroLog.Internal
{
    internal class Logger : ILogger, ILoggerAsync, ILoggerQuery
    {
        private static readonly Task<LogWriteOperation[]> EmptyOperations = Task.FromResult(new LogWriteOperation[] { });
        private readonly LoggingConfiguration configuration;

        public Logger(string name, LoggingConfiguration configuration)
        {
            this.Name = name;
            this.configuration = configuration;
        }

        internal Task<LogWriteOperation[]> TraceAsync(string message, Exception ex = null)
        {
            return this.LogAsync(LogLevel.Trace, message, ex);
        }

        internal Task<LogWriteOperation[]> TraceAsync(string message, params object[] ps)
        {
            return this.LogAsync(LogLevel.Trace, message, ps);
        }

        internal Task<LogWriteOperation[]> DebugAsync(string message, Exception ex = null)
        {
            return this.LogAsync(LogLevel.Debug, message, ex);
        }

        internal Task<LogWriteOperation[]> DebugAsync(string message, params object[] ps)
        {
            return this.LogAsync(LogLevel.Debug, message, ps);
        }

        internal Task<LogWriteOperation[]> InfoAsync(string message, Exception ex = null)
        {
            return this.LogAsync(LogLevel.Info, message, ex);
        }

        internal Task<LogWriteOperation[]> InfoAsync(string message, params object[] ps)
        {
            return this.LogAsync(LogLevel.Info, message, ps);
        }

        internal Task<LogWriteOperation[]> WarnAsync(string message, Exception ex = null)
        {
            return this.LogAsync(LogLevel.Warn, message, ex);
        }

        internal Task<LogWriteOperation[]> WarnAsync(string message, params object[] ps)
        {
            return this.LogAsync(LogLevel.Warn, message, ps);
        }

        internal Task<LogWriteOperation[]> ErrorAsync(string message, Exception ex = null)
        {
            return this.LogAsync(LogLevel.Error, message, ex);
        }

        internal Task<LogWriteOperation[]> ErrorAsync(string message, params object[] ps)
        {
            return this.LogAsync(LogLevel.Error, message, ps);
        }

        internal Task<LogWriteOperation[]> FatalAsync(string message, Exception ex = null)
        {
            return this.LogAsync(LogLevel.Fatal, message, ex);
        }

        internal Task<LogWriteOperation[]> FatalAsync(string message, params object[] ps)
        {
            return this.LogAsync(LogLevel.Fatal, message, ps);
        }

        internal Task<LogWriteOperation[]> LogAsync(LogLevel logLevel, string message, Exception ex)
        {
            return this.LogAsyncInternal(logLevel, message, null, ex, false);
        }

        internal Task<LogWriteOperation[]> LogAsync(LogLevel logLevel, string message, params object[] ps)
        {
            return this.LogAsyncInternal(logLevel, message, ps, null, true);
        }

        private Task<LogWriteOperation[]> LogAsyncInternal(LogLevel level, string message, object[] ps, Exception ex, bool doFormat)
        {
            try
            {
                if (this.configuration.IsEnabled == false)
                {
                    return EmptyOperations;
                }

                // format?
                if (doFormat)
                {
                    message = string.Format(message, ps);
                }

                // create an event entry and pass it through...
                var entry = new LogEventInfo(level, this.Name, message, ex);
                var isFatalException = level == LogLevel.Fatal;

                var crashRecorder = this.configuration.CrashRecorder;
                if (crashRecorder.IsEnabled)
                {
                    if (isFatalException)
                    {
                        var records = crashRecorder.GetRecords();
                        entry.CrashRecords = records;
                    }
                    else
                    {
                        crashRecorder.Record(entry);
                    }
                }

                var targets = this.configuration.GetTargets(level);
                if (!targets.Any())
                {
                    return EmptyOperations;
                }

                // create a context...
                var context = new LogWriteContext();
                context.IsFatalException = isFatalException;


                // gather the tasks...
                var writeTasks = from target in targets 
                                 select target.WriteAsync(context, entry);

                // group...
                var group = Task.WhenAll(writeTasks);
                return group;
            }
            catch (Exception logEx)
            {
                InternalLogger.Current.Error("Logging operation failed.", logEx);
                return EmptyOperations;
            }
        }

        public string Name { get; private set; }

        public bool IsTraceEnabled
        {
            get
            {
                return this.IsEnabled(LogLevel.Trace);
            }
        }

        public bool IsDebugEnabled
        {
            get
            {
                return this.IsEnabled(LogLevel.Debug);
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                return this.IsEnabled(LogLevel.Info);
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                return this.IsEnabled(LogLevel.Warn);
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                return this.IsEnabled(LogLevel.Error);
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
                return this.IsEnabled(LogLevel.Fatal);
            }
        }

        public void Trace(string message, Exception ex = null)
        {
            this.TraceAsync(message, ex);
        }

        public void Trace(string message, params object[] ps)
        {
            this.TraceAsync(message, ps);
        }

        public void Debug(string message, Exception ex = null)
        {
            this.DebugAsync(message, ex);
        }

        public void Debug(string message, params object[] ps)
        {
            this.DebugAsync(message, ps);
        }

        public void Info(string message, Exception ex = null)
        {
            this.InfoAsync(message, ex);
        }

        public void Info(string message, params object[] ps)
        {
            this.InfoAsync(message, ps);
        }

        public void Warn(string message, Exception ex = null)
        {
            this.WarnAsync(message, ex);
        }

        public void Warn(string message, params object[] ps)
        {
            this.WarnAsync(message, ps);
        }

        public void Error(string message, Exception ex = null)
        {
            this.ErrorAsync(message, ex);
        }

        public void Error(string message, params object[] ps)
        {
            this.ErrorAsync(message, ps);
        }

        public void Fatal(string message, Exception ex = null)
        {
            this.FatalAsync(message, ex);
        }

        public void Fatal(string message, params object[] ps)
        {
            this.FatalAsync(message, ps);
        }

        public void Log(LogLevel logLevel, string message, Exception ex)
        {
            this.LogAsync(logLevel, message, ex);
        }

        public void Log(LogLevel logLevel, string message, params object[] ps)
        {
            this.LogAsync(logLevel, message, ps);
        }

        public bool IsEnabled(LogLevel level)
        {
            return this.configuration.GetTargets(level).Any();
        }

        Task<LogWriteOperation[]> ILoggerAsync.TraceAsync(string message, Exception ex)
        {
            return this.TraceAsync(message, ex);
        }

        Task<LogWriteOperation[]> ILoggerAsync.TraceAsync(string message, params object[] ps)
        {
            return this.TraceAsync(message, ps);
        }

        Task<LogWriteOperation[]> ILoggerAsync.DebugAsync(string message, Exception ex)
        {
            return this.DebugAsync(message, ex);
        }

        Task<LogWriteOperation[]> ILoggerAsync.DebugAsync(string message, params object[] ps)
        {
            return this.DebugAsync(message, ps);
        }

        Task<LogWriteOperation[]> ILoggerAsync.InfoAsync(string message, Exception ex)
        {
            return this.InfoAsync(message, ex);
        }

        Task<LogWriteOperation[]> ILoggerAsync.InfoAsync(string message, params object[] ps)
        {
            return this.InfoAsync(message, ps);
        }

        Task<LogWriteOperation[]> ILoggerAsync.WarnAsync(string message, Exception ex)
        {
            return this.WarnAsync(message, ex);
        }

        Task<LogWriteOperation[]> ILoggerAsync.WarnAsync(string message, params object[] ps)
        {
            return this.WarnAsync(message, ps);
        }

        Task<LogWriteOperation[]> ILoggerAsync.ErrorAsync(string message, Exception ex)
        {
            return this.ErrorAsync(message, ex);
        }

        Task<LogWriteOperation[]> ILoggerAsync.ErrorAsync(string message, params object[] ps)
        {
            return this.ErrorAsync(message, ps);
        }

        Task<LogWriteOperation[]> ILoggerAsync.FatalAsync(string message, Exception ex)
        {
            return this.FatalAsync(message, ex);
        }

        Task<LogWriteOperation[]> ILoggerAsync.FatalAsync(string message, params object[] ps)
        {
            return this.FatalAsync(message, ps);
        }

        Task<LogWriteOperation[]> ILoggerAsync.LogAsync(LogLevel logLevel, string message, Exception ex)
        {
            return this.LogAsync(logLevel, message, ex);
        }

        Task<LogWriteOperation[]> ILoggerAsync.LogAsync(LogLevel logLevel, string message, params object[] ps)
        {
            return this.LogAsync(logLevel, message, ps);
        }

        public IEnumerable<Target> GetTargets()
        {
            return this.configuration.GetTargets();
        }
    }
}