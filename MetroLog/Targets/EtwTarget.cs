using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Layouts;

namespace MetroLog.Targets
{
    public class EtwTarget : SyncTarget
    {
         public EtwTarget()
            : this(new SingleLineLayout())
        {
        }

         public EtwTarget(Layout layout)
            : base(layout)
        {
        }

        protected override void Write(LogEventInfo entry)
        {
            var message = Layout.GetFormattedString(entry);

            switch(entry.Level)
            {
                case LogLevel.Trace:
                    MetroLogEventSource.Log.Trace(message);
                    break;
                case LogLevel.Debug:
                    MetroLogEventSource.Log.Debug(message);
                    break;
                case LogLevel.Info:
                    MetroLogEventSource.Log.Info(message);
                    break;
                case LogLevel.Warn:
                    MetroLogEventSource.Log.Warn(message);
                    break;
                case LogLevel.Error:
                    MetroLogEventSource.Log.Error(message);
                    break;
                case LogLevel.Fatal:
                    MetroLogEventSource.Log.Fatal(message);
                    break;
            }
        }
    }

    internal sealed class MetroLogEventSource : EventSource
    {
        public static readonly MetroLogEventSource Log = new MetroLogEventSource();

// ReSharper disable InconsistentNaming

        public void Info(string Message)
        {
            WriteEvent(1, Message, Name);
        }

        public void Trace(string Message)
        {
            WriteEvent(2, Message, Name);
        }

        public void Warn(string Message)
        {
            WriteEvent(3, Message, Name);
        }

        public void Error(string Message)
        {
            WriteEvent(4, Message, Name);
        }

        public void Fatal(string Message)
        {
            WriteEvent(5, Message, Name);
        }

        public void Debug(string Message)
        {
            WriteEvent(6, Message, Name);
        }

// ReSharper restore InconsistentNaming
    }
}
