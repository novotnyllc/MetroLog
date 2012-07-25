using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    public static class ILoggableExtender
    {
        public static Task<LogWriteOperation[]> Trace(this ILoggable loggable, string message, Exception ex = null)
        {
            var logger = loggable.GetLogger();
            return logger.Log(LogLevel.Trace, message, ex);
        }

        public static Task<LogWriteOperation[]> Trace(this ILoggable loggable, string message, params string[] ps)
        {
            var logger = loggable.GetLogger();
            return logger.Log(LogLevel.Trace, message, ps);
        }

        public static Task<LogWriteOperation[]> Debug(this ILoggable loggable, string message, Exception ex = null)
        {
            var logger = loggable.GetLogger();
            return logger.Log(LogLevel.Debug, message, ex);
        }

        public static Task<LogWriteOperation[]> Debug(this ILoggable loggable, string message, params string[] ps)
        {
            var logger = loggable.GetLogger();
            return logger.Log(LogLevel.Debug, message, ps);
        }

        public static Task<LogWriteOperation[]> Info(this ILoggable loggable, string message, Exception ex = null)
        {
            var logger = loggable.GetLogger();
            return logger.Log(LogLevel.Info, message, ex);
        }

        public static Task<LogWriteOperation[]> Info(this ILoggable loggable, string message, params object[] ps)
        {
            var logger = loggable.GetLogger();
            return logger.Log(LogLevel.Info, message, ps);
        }

        public static Task<LogWriteOperation[]> Warn(this ILoggable loggable, string message, Exception ex = null)
        {
            var logger = loggable.GetLogger();
            return logger.Log(LogLevel.Warn, message, ex);
        }

        public static Task<LogWriteOperation[]> Warn(this ILoggable loggable, string message, params string[] ps)
        {
            var logger = loggable.GetLogger();
            return logger.Log(LogLevel.Warn, message, ps);
        }

        public static Task<LogWriteOperation[]> Error(this ILoggable loggable, string message, Exception ex = null)
        {
            var logger = loggable.GetLogger();
            return logger.Log(LogLevel.Error, message, ex);
        }

        public static Task<LogWriteOperation[]> Error(this ILoggable loggable, string message, params string[] ps)
        {
            var logger = loggable.GetLogger();
            return logger.Log(LogLevel.Error, message, ps);
        }

        public static Task<LogWriteOperation[]> Fatal(this ILoggable loggable, string message, Exception ex = null)
        {
            var logger = loggable.GetLogger();
            return logger.Log(LogLevel.Fatal, message, ex);
        }

        public static Task<LogWriteOperation[]> Fatal(this ILoggable loggable, string message, params string[] ps)
        {
            var logger = loggable.GetLogger();
            return logger.Log(LogLevel.Fatal, message, ps);
        }

        public static Logger GetLogger(this ILoggable loggable, LoggingConfiguration config = null)
        {
            return LogManager.GetLogger(loggable, config);
        }

        public static bool IsTraceEnabled(this ILoggable loggable)
        {
            return loggable.IsLevelEnabled(LogLevel.Trace);
        }

        public static bool IsDebugEnabled(this ILoggable loggable)
        {
            return loggable.IsLevelEnabled(LogLevel.Debug);
        }

        public static bool IsInfoEnabled(this ILoggable loggable)
        {
            return loggable.IsLevelEnabled(LogLevel.Info);
        }

        public static bool IsWarnEnabled(this ILoggable loggable)
        {
            return loggable.IsLevelEnabled(LogLevel.Warn);
        }

        public static bool IsErrorEnabled(this ILoggable loggable)
        {
            return loggable.IsLevelEnabled(LogLevel.Error);
        }

        public static bool IsFatalEnabled(this ILoggable loggable)
        {
            return loggable.IsLevelEnabled(LogLevel.Fatal);
        }

        public static bool IsLevelEnabled(this ILoggable loggable, LogLevel level)
        {
            var logger = loggable.GetLogger();
            return logger.IsEnabled(level);
        }
    }
}
