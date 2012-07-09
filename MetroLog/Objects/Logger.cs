using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Trace(string message, Exception ex = null)
        {
            Log(LogLevel.Trace, message, ex);
        }

        public void Trace(string message, params string[] ps)
        {
            Log(LogLevel.Trace, message, ps);
        }

        public void Debug(string message, Exception ex = null)
        {
            Log(LogLevel.Debug, message, ex);
        }

        public void Debug(string message, params string[] ps)
        {
            Log(LogLevel.Debug, message, ps);
        }

        public void Info(string message, Exception ex = null)
        {
            Log(LogLevel.Info, message, ex);
        }

        public void Info(string message, params string[] ps)
        {
            Log(LogLevel.Info, message, ps);
        }

        public void Warn(string message, Exception ex = null)
        {
            Log(LogLevel.Warn, message, ex);
        }

        public void Warn(string message, params string[] ps)
        {
            Log(LogLevel.Warn, message, ps);
        }

        public void Error(string message, Exception ex = null)
        {
            Log(LogLevel.Error, message, ex);
        }

        public void Error(string message, params string[] ps)
        {
            Log(LogLevel.Error, message, ps);
        }

        public void Fatal(string message, Exception ex = null)
        {
            Log(LogLevel.Fatal, message, ex);
        }

        public void Fatal(string message, params string[] ps)
        {
            Log(LogLevel.Fatal, message, ps);
        }

        public LogEventInfo Log(LogLevel logLevel, string message, Exception ex)
        {
            return LogInternal(logLevel, message, null, ex, false);
        }

        public LogEventInfo Log(LogLevel logLevel, string message, params string[] ps)
        {
            return LogInternal(logLevel, message, ps, null, true);
        }

        private LogEventInfo LogInternal(LogLevel level, string message, string[] ps, Exception ex, bool doFormat)
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
            foreach (var target in targets)
            {
                try
                {
                    target.Write(entry);
                }
                catch (Exception writeEx)
                {
                    LogManager.LogInternal(string.Format("Failed to write to target '{0}'.", target), writeEx);
                }
            }

            // return...
            return entry;
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
