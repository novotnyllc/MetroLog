using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Targets;

namespace MetroLog
{
    public class Logger
    {
        private string Name { get; set; }
        private LoggingConfiguration Configuration { get; set; }

        internal Logger(string name, LoggingConfiguration config)
        {
            this.Name = name;

            // add a target...
            if (config != null)
                this.Configuration = config.Clone();
        }

        private LoggingConfiguration ResolvedConfiguration
        {
            get
            {
                // do we have one?
                if (this.Configuration != null)
                    return this.Configuration;
                else
                    return LogManager.DefaultConfiguration;
            }
        }

        public Task<LogWriteOperation[]> Trace(string message, Exception ex = null)
        {
            return Log(LogLevel.Trace, message, ex);
        }

        public Task<LogWriteOperation[]> Trace(string message, params string[] ps)
        {
            return Log(LogLevel.Trace, message, ps);
        }

        public Task<LogWriteOperation[]> Debug(string message, Exception ex = null)
        {
            return Log(LogLevel.Debug, message, ex);
        }

        public Task<LogWriteOperation[]> Debug(string message, params string[] ps)
        {
            return Log(LogLevel.Debug, message, ps);
        }

        public Task<LogWriteOperation[]> Info(string message, Exception ex = null)
        {
            return Log(LogLevel.Info, message, ex);
        }

        public Task<LogWriteOperation[]> Info(string message, params string[] ps)
        {
            return Log(LogLevel.Info, message, ps);
        }

        public Task<LogWriteOperation[]> Warn(string message, Exception ex = null)
        {
            return Log(LogLevel.Warn, message, ex);
        }

        public Task<LogWriteOperation[]> Warn(string message, params string[] ps)
        {
            return Log(LogLevel.Warn, message, ps);
        }

        public Task<LogWriteOperation[]> Error(string message, Exception ex = null)
        {
            return Log(LogLevel.Error, message, ex);
        }

        public Task<LogWriteOperation[]> Error(string message, params string[] ps)
        {
            return Log(LogLevel.Error, message, ps);
        }

        public Task<LogWriteOperation[]> Fatal(string message, Exception ex = null)
        {
            return Log(LogLevel.Fatal, message, ex);
        }

        public Task<LogWriteOperation[]> Fatal(string message, params string[] ps)
        {
            return Log(LogLevel.Fatal, message, ps);
        }

        public Task<LogWriteOperation[]> Log(LogLevel logLevel, string message, Exception ex)
        {
            return LogInternal(logLevel, message, null, ex, false);
        }

        public Task<LogWriteOperation[]> Log(LogLevel logLevel, string message, params object[] ps)
        {
            return LogInternal(logLevel, message, ps, null, true);
        }

        private Task<LogWriteOperation[]> LogInternal(LogLevel level, string message, object[] ps, Exception ex, bool doFormat)
        {
            try
            {
                var targets = this.ResolvedConfiguration.GetTargets(level);
                if (!(targets.Any()))
                    return null;

                // format?
                if (doFormat)
                    message = string.Format(message, ps);

                // create an event entry and pass it through...
                var entry = new LogEventInfo(level, this.Name, message, ex);

                // do the sync ones...
                var tasks = new List<Task<LogWriteOperation>>();
                foreach (var target in targets)
                    tasks.Add(target.WriteAsync(entry));

                // group...
                var group = Task.WhenAll<LogWriteOperation>(tasks);
                return group;
            }
            catch (Exception logEx)
            {
                LogManager.LogInternal("Logging operation failed.", logEx);
                return Task.FromResult<LogWriteOperation[]>(new LogWriteOperation[] {});
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

        public bool IsEnabled(LogLevel level)
        {
            return this.ResolvedConfiguration.GetTargets(level).Any();
        }
    }
}
