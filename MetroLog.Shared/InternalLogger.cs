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
        static readonly InternalLogger current;

        InternalLogger()
        {
        }

        static InternalLogger()
        {
            current = new InternalLogger();
        }

        public static ILogger Current => current;

        public string Name => "(Internal)";

        public bool IsTraceEnabled => true;
        public bool IsDebugEnabled => true;
        public bool IsInfoEnabled => true;
        public bool IsWarnEnabled => true;
        public bool IsErrorEnabled => true;
        public bool IsFatalEnabled => true;

        public void Trace(string message, Exception ex = null)
        {
            Log(LogLevel.Trace, message, ex);
        }

        public void Trace(string message, params object[] ps)
        {
            Log(LogLevel.Trace, message, ps);
        }

        public void Debug(string message, Exception ex = null)
        {
            Log(LogLevel.Trace, message, ex);
        }

        public void Debug(string message, params object[] ps)
        {
            Log(LogLevel.Trace, message, ps);
        }

        public void Info(string message, Exception ex = null)
        {
            Log(LogLevel.Trace, message, ex);
        }

        public void Info(string message, params object[] ps)
        {
            Log(LogLevel.Trace, message, ps);
        }

        public void Warn(string message, Exception ex = null)
        {
            Log(LogLevel.Trace, message, ex);
        }

        public void Warn(string message, params object[] ps)
        {
            Log(LogLevel.Trace, message, ps);
        }

        public void Error(string message, Exception ex = null)
        {
            Log(LogLevel.Trace, message, ex);
        }

        public void Error(string message, params object[] ps)
        {
            Log(LogLevel.Trace, message, ps);
        }

        public void Fatal(string message, Exception ex = null)
        {
            Log(LogLevel.Trace, message, ex);
        }

        public void Fatal(string message, params object[] ps)
        {
            Log(LogLevel.Trace, message, ps);
        }

        public void Log(LogLevel logLevel, string message, Exception ex)
        {
            string formatted = null;
            long sequence = LogEventInfo.GetNextSequenceId();
            string dt = LogManager.GetDateTime().ToString(LogManager.DateTimeFormat);
            string asString = logLevel.ToString().ToUpper();
            int thread = Environment.CurrentManagedThreadId;
            formatted = ex != null ? $"{sequence}|{dt}|{asString}|{thread}|{message} --> {ex}" : 
                                     $"{sequence}|{dt}|{asString}|{thread}|{message}";

            // debug...
            System.Diagnostics.Debug.WriteLine(formatted);
            // TODO: EWT
        }

        public void Log(LogLevel logLevel, string message, params object[] ps)
        {
            if(ps.Any())
                Log(logLevel, string.Format(message, ps), (Exception)null);
            else
                Log(logLevel, message, (Exception)null);
        }

        public bool IsEnabled(LogLevel level)
        {
            return true;
        }
    }
}
