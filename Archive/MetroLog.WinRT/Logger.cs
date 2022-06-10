using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetroLog.Targets;
using Windows.Foundation;
using Windows.Storage;
using IPclLogger = MetroLog.ILogger;
using PclLogLevel = MetroLog.LogLevel;

namespace MetroLog.WinRT
{
    public sealed class Logger : ILogger
    {
        readonly IPclLogger _logger;
        static readonly Lazy<IWinRTLogManager> _logManager;
        public static event EventHandler<string> OnLogMessage;

        static readonly SynchronizationContext _context = SynchronizationContext.Current;

        static Logger()
        {
            MaxLevel = LogLevel.Trace;


            _logManager = new Lazy<IWinRTLogManager>(() =>
                {
                    var max = (PclLogLevel)MaxLevel;
                    // Log everything for now
                    var configuration = new LoggingConfiguration();
                    configuration.AddTarget(max, PclLogLevel.Fatal, new DebugTarget());
                    configuration.AddTarget(max, PclLogLevel.Fatal, new EtwTarget());
                    configuration.AddTarget(max, PclLogLevel.Fatal, new EventTarget(OnLogMessageInternal));
                    configuration.AddTarget(max, PclLogLevel.Fatal, new StreamingFileTarget());

                    LogManagerFactory.DefaultConfiguration = configuration;

                    return (IWinRTLogManager)LogManagerFactory.DefaultLogManager;
                });
        }

        /// <summary>
        /// Maximum verbosity to be returned. Default is all, which is Trace. Must be set prior to first access of GetLogger
        /// </summary>
        public static LogLevel MaxLevel { get; set; }

        /// <summary>
        /// Returns a zip file of the compressed logs
        /// </summary>
        /// <returns></returns>
        public static IAsyncOperation<IStorageFile> GetCompressedLogFile()
        {
            return _logManager.Value.GetCompressedLogFile().AsAsyncOperation();
        }

        static void OnLogMessageInternal(string message)
        {
            var evt = OnLogMessage;
            if (evt != null)
            {
                _context.Post(_ => evt(null, message), null);
            }
        }

        Logger(IPclLogger logger)
        {
            _logger = logger;
        }

        public static ILogger GetLogger(string name)
        {
            return new Logger(_logManager.Value.GetLogger(name));
        }


        public string Name
        {
            get { return _logger.Name; }
        }

        public bool IsTraceEnabled
        {
            get { return _logger.IsTraceEnabled; }
        }
        public bool IsDebugEnabled
        {
            get { return _logger.IsDebugEnabled; }
        }
        public bool IsInfoEnabled
        {
            get { return _logger.IsInfoEnabled; }
        }
        public bool IsWarnEnabled
        {
            get { return _logger.IsWarnEnabled; }
        }
        public bool IsErrorEnabled
        {
            get { return _logger.IsErrorEnabled; }
        }
        public bool IsFatalEnabled
        {
            get { return _logger.IsFatalEnabled; }
        }

        public bool IsEnabled(LogLevel level)
        {
            return _logger.IsEnabled((PclLogLevel)level);
        }

        public void Trace(string message, [ReadOnlyArray] object[] ps)
        {
            _logger.Trace(message, ps);
        }

        public void Debug(string message, [ReadOnlyArray] object[] ps)
        {
            _logger.Debug(message, ps);
        }

        public void Info(string message, [ReadOnlyArray] object[] ps)
        {
            _logger.Info(message, ps);
        }

        public void Warn(string message, [ReadOnlyArray] object[] ps)
        {
            _logger.Warn(message, ps);
        }

        public void Error(string message, [ReadOnlyArray] object[] ps)
        {
            _logger.Error(message, ps);
        }

        public void Fatal(string message, [ReadOnlyArray] object[] ps)
        {
            _logger.Fatal(message, ps);
        }

        public void Trace(string message)
        {
            _logger.Trace(message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public void Log(LogLevel logLevel, string message, [ReadOnlyArray] object[] ps)
        {
            _logger.Log((PclLogLevel)logLevel, message, ps);
        }

        public void Log(LogLevel logLevel, string message)
        {
            _logger.Log((PclLogLevel)logLevel, message);
        }
    }
}
