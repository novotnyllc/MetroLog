using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Layouts;
using MetroLog.Targets;

namespace MetroLog
{
    public static class LogManager
    {
        public static LoggingConfiguration DefaultConfiguration { get; set; }

        private static Dictionary<string, Logger> Loggers { get; set; }
        private static object _loggersLock = new object();

        internal const string DateTimeFormat = "dd/MMM/yyyy HH:mm:ss";

        static LogManager()
        {
            Reset();
        }

        public static Logger GetLogger(ILoggable loggable, LoggingConfiguration config)
        {
            return GetLogger(loggable.GetType().Name, config);
        }

        public static Logger GetLogger(string name, LoggingConfiguration config = null)
        {
            lock (_loggersLock)
            {
                if (!(Loggers.ContainsKey(name)))
                    Loggers[name] = new Logger(name, config);
                return Loggers[name];
            }
        }

        public static void Reset()
        {
            Loggers = new Dictionary<string, Logger>(StringComparer.OrdinalIgnoreCase);

            // default logging config...
            var config = new LoggingConfiguration();
            config.AddTarget(LogLevel.Trace, LogLevel.Fatal, new DebugTarget());
            config.AddTarget(LogLevel.Error, LogLevel.Fatal, new FileSnapshotTarget());
            DefaultConfiguration = config;
        }

        // logs problems with the framework to Debug...
        internal static void LogInternal(string message, Exception ex)
        {
            if(ex != null)
                Debug.WriteLine("{0}|INTERNAL|(null)|{1} --> {2}", GetDateTime().ToString(DateTimeFormat), message, ex);
            else
                Debug.WriteLine("{0}|INTERNAL|(null)|{1}", GetDateTime().ToString(DateTimeFormat), message);
        }

        internal static DateTime GetDateTime()
        {
            // NLog has a high efficiency version of this...
            return DateTime.Now;
        }
    }
}
