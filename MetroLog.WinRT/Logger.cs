using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetroLog.Targets;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using IPclLogger = MetroLog.ILogger;
using PclLogLevel = MetroLog.LogLevel;

namespace MetroLog.WinRT
{
    public sealed class Logger : ILogger
    {
        private readonly IPclLogger _logger;
        private static readonly Lazy<ILogManager> _logManager;
        public static event EventHandler<string> OnLogMessage;

        private static readonly SynchronizationContext _context = SynchronizationContext.Current;

        static Logger()
        {
            _logManager = new Lazy<ILogManager>(() =>
                {
                    // Log everything for now
                    var configuration = new LoggingConfiguration();
                    configuration.AddTarget(PclLogLevel.Trace, PclLogLevel.Fatal, new DebugTarget());
                    configuration.AddTarget(PclLogLevel.Trace, PclLogLevel.Fatal, new EtwTarget());
                    configuration.AddTarget(PclLogLevel.Trace, PclLogLevel.Fatal, new EventTarget(OnLogMessageInternal));
                    configuration.AddTarget(PclLogLevel.Trace, PclLogLevel.Fatal, new FileStreamingTarget());

                    LogManagerFactory.DefaultConfiguration = configuration;

                    return LogManagerFactory.DefaultLogManager;
                });
        }

        /// <summary>
        /// Returns a zip file stream of compressed logs
        /// </summary>
        /// <returns></returns>
        public static IAsyncOperation<IRandomAccessStream> GetCompressedLogs()
        {
            return GetCompressedLogsInternal().AsAsyncOperation();
        }

        private static async Task<IRandomAccessStream> GetCompressedLogsInternal()
        {
            using (var stream = await _logManager.Value.GetCompressedLogs())
            {
                // Copy to a WinRT buffer
                var dest = new InMemoryRandomAccessStream();
                await stream.CopyToAsync(dest.AsStreamForWrite());
                dest.Seek(0);

                return dest;
            }
        }

        private static void OnLogMessageInternal(string message)
        {
            var evt = OnLogMessage;
            if (evt != null)
            {
                _context.Post(_ => evt(null, message), null);
            }
        }

        private Logger(IPclLogger logger)
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
