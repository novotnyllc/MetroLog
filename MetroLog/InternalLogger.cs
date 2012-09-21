#define DEBUG // for debug.writeline

using MetroLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    public class InternalLogger : ILogger
    {
        private static readonly InternalLogger _current;

        private InternalLogger()
        {
        }

        static InternalLogger()
        {
            _current = new InternalLogger();
        }

        public static ILogger Current
        {
            get
            {
                return _current;
            }
        }

        public string Name
        {
            get
            {
                return "(Internal)";
            }
        }

        public bool IsTraceEnabled { get { return true; } }
        public bool IsDebugEnabled { get { return true; } }
        public bool IsInfoEnabled { get { return true; } }
        public bool IsWarnEnabled { get { return true; } }
        public bool IsErrorEnabled { get { return true; } }
        public bool IsFatalEnabled { get { return true; } }

        public void Trace(string message, Exception ex = null)
        {
            this.Log(LogLevel.Trace, message, ex);
        }

        public void Trace(string message, params object[] ps)
        {
            this.Log(LogLevel.Trace, message, ps);
        }

        public void Debug(string message, Exception ex = null)
        {
            this.Log(LogLevel.Trace, message, ex);
        }

        public void Debug(string message, params object[] ps)
        {
            this.Log(LogLevel.Trace, message, ps);
        }

        public void Info(string message, Exception ex = null)
        {
            this.Log(LogLevel.Trace, message, ex);
        }

        public void Info(string message, params object[] ps)
        {
            this.Log(LogLevel.Trace, message, ps);
        }

        public void Warn(string message, Exception ex = null)
        {
            this.Log(LogLevel.Trace, message, ex);
        }

        public void Warn(string message, params object[] ps)
        {
            this.Log(LogLevel.Trace, message, ps);
        }

        public void Error(string message, Exception ex = null)
        {
            this.Log(LogLevel.Trace, message, ex);
        }

        public void Error(string message, params object[] ps)
        {
            this.Log(LogLevel.Trace, message, ps);
        }

        public void Fatal(string message, Exception ex = null)
        {
            this.Log(LogLevel.Trace, message, ex);
        }

        public void Fatal(string message, params object[] ps)
        {
            this.Log(LogLevel.Trace, message, ps);
        }

        public void Log(LogLevel logLevel, string message, Exception ex)
        {
            string formatted = null;
            long sequence = LogEventInfo.GetNextSequenceId();
            string dt = LogManager.GetDateTime().ToString(LogManager.DateTimeFormat);
            string asString = logLevel.ToString().ToUpper();
            int thread = Environment.CurrentManagedThreadId;
            if (ex != null)
                formatted = string.Format("{0}|{1}|{2}|{3}|{4} --> {5}", sequence, dt, asString, thread, message, ex);
            else
                formatted = string.Format("{0}|{1}|{2}|{3}|{4}", sequence, dt, asString, thread, message);

            // debug...
            System.Diagnostics.Debug.WriteLine(formatted);
            // TODO: EWT
        }

        public void Log(LogLevel logLevel, string message, params object[] ps)
        {
            if(ps.Any())
                this.Log(logLevel, string.Format(message, ps), (Exception)null);
            else
                this.Log(logLevel, message, (Exception)null);
        }

        public bool IsEnabled(LogLevel level)
        {
            return true;
        }
    }
}
