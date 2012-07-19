using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MetroLog
{
    public class LogEventInfo
    {
        public LogLevel Level { get; private set; }
        public string Logger { get; private set; }
        public string Message { get; private set; }
        public Exception Exception { get; private set; }
        public long SequenceID { get; private set; }
        public DateTime TimeStamp { get; private set; }

        private static long _globalSequenceId;

        internal LogEventInfo(LogLevel level, string logger, string message, Exception ex)
        {
            this.Level = level;
            this.Logger = logger;
            this.Message = message;
            this.Exception = ex;
            this.TimeStamp = LogManager.GetDateTime();
            this.SequenceID = Interlocked.Increment(ref _globalSequenceId);
        }

        internal LogEventInfo Clone()
        {
            var newInfo = new LogEventInfo(this.Level, this.Logger, this.Message, this.Exception);
            newInfo.TimeStamp = this.TimeStamp;
            newInfo.SequenceID = this.SequenceID;

            // return...
            return newInfo;
        }
    }
}
