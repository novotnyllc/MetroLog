using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.WinRT
{
    public interface ILogger
    {
        string Name { get; }
        bool IsTraceEnabled { get; }
        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }
        bool IsEnabled(LogLevel level);

        void Trace(string message, [ReadOnlyArray] object[] ps);
        void Debug(string message, [ReadOnlyArray] object[] ps);
        void Info(string message, [ReadOnlyArray] object[] ps);
        void Warn(string message, [ReadOnlyArray] object[] ps);
        void Error(string message, [ReadOnlyArray] object[] ps);
        void Fatal(string message, [ReadOnlyArray] object[] ps);

        void Trace(string message);
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Fatal(string message);


        void Log(LogLevel logLevel, string message, [ReadOnlyArray] object[] ps);
        void Log(LogLevel logLevel, string message);
    }

    public enum LogLevel
    {
        Trace = 0,
        Debug = 1,
        Info = 2,
        Warn = 3,
        Error = 4,
        Fatal = 5
    }
}
