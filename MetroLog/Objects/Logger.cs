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

        public LogWriteOperation Trace(string message, Exception ex = null)
        {
            return Log(LogLevel.Trace, message, ex);
        }

        public LogWriteOperation Trace(string message, params string[] ps)
        {
            return Log(LogLevel.Trace, message, ps);
        }

        public LogWriteOperation Debug(string message, Exception ex = null)
        {
            return Log(LogLevel.Debug, message, ex);
        }

        public LogWriteOperation Debug(string message, params string[] ps)
        {
            return Log(LogLevel.Debug, message, ps);
        }

        public LogWriteOperation Info(string message, Exception ex = null)
        {
            return Log(LogLevel.Info, message, ex);
        }

        public LogWriteOperation Info(string message, params string[] ps)
        {
            return Log(LogLevel.Info, message, ps);
        }

        public LogWriteOperation Warn(string message, Exception ex = null)
        {
            return Log(LogLevel.Warn, message, ex);
        }

        public LogWriteOperation Warn(string message, params string[] ps)
        {
            return Log(LogLevel.Warn, message, ps);
        }

        public LogWriteOperation Error(string message, Exception ex = null)
        {
            return Log(LogLevel.Error, message, ex);
        }

        public LogWriteOperation Error(string message, params string[] ps)
        {
            return Log(LogLevel.Error, message, ps);
        }

        public LogWriteOperation Fatal(string message, Exception ex = null)
        {
            return Log(LogLevel.Fatal, message, ex);
        }

        public LogWriteOperation Fatal(string message, params string[] ps)
        {
            return Log(LogLevel.Fatal, message, ps);
        }

        public LogWriteOperation Log(LogLevel logLevel, string message, Exception ex)
        {
            return LogInternal(logLevel, message, null, ex, false);
        }

        public LogWriteOperation Log(LogLevel logLevel, string message, params string[] ps)
        {
            return LogInternal(logLevel, message, ps, null, true);
        }

        private LogWriteOperation LogInternal(LogLevel level, string message, string[] ps, Exception ex, bool doFormat)
        {
            var targets = this.ResolvedConfiguration.GetTargets(level);
            if(!(targets.Any()))
                return null;

            // format?
            if (doFormat)
            {
                try
                {
                    message = string.Format(message, ps);
                }
                catch (Exception formatEx)
                {
                    LogManager.LogInternal(string.Format("Failed to format message with format '{0}'.", message), formatEx);
                    return null;
                }
            }

            // create an event entry and pass it through...
            var entry = new LogEventInfo(level, this.Name, message, ex);

            // do the sync ones...
            var asyncTargets = new List<AsyncTarget>();
            foreach (var target in targets)
            {
                try
                {
                    if(target is AsyncTarget)
                        asyncTargets.Add((AsyncTarget)target);
                    else if(target is SyncTarget)
                        ((SyncTarget)target).WriteSync(entry);
                    else
                        throw new NotSupportedException(string.Format("Cannot handle '{0}'.", target.GetType()));
                }
                catch (Exception writeEx)
                {
                    LogManager.LogInternal(string.Format("Failed to write to target '{0}'.", target), writeEx);
                }
            }

            // spin up async targets?
            if (asyncTargets.Any())
            {
                var task = Task.Run(async () =>
                {
                    var tasks = new List<Task>();
                    foreach (var asyncTarget in asyncTargets)
                        tasks.Add(asyncTarget.WriteAsync(entry));

                    // walk...
                    await Task.WhenAll(tasks);
                });
                return new LogWriteOperation(entry, task);
            }
            else
                return new LogWriteOperation(entry);
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
