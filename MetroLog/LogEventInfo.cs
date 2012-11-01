using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetroLog.Internal;

namespace MetroLog
{
    public class LogEventInfo
    {
        public long SequenceID { get; set; }
        public LogLevel Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

        [JsonIgnore]
        public Exception Exception { get; set; }

        private ExceptionWrapper _exceptionWrapper;

        private static long _globalSequenceId;

        public LogEventInfo(LogLevel level, string logger, string message, Exception ex)
        {
            Level = level;
            Logger = logger;
            Message = message;
            Exception = ex;
            TimeStamp = LogManagerBase.GetDateTime();
            SequenceID = GetNextSequenceId();
        }

        internal static long GetNextSequenceId()
        {
            return Interlocked.Increment(ref _globalSequenceId);
        }

        public string ToJson()
        {
            return SimpleJson.SerializeObject(this);
        }

        public ExceptionWrapper ExceptionWrapper
        {
            get
            {
                if (_exceptionWrapper == null && this.Exception != null)
                    _exceptionWrapper = new ExceptionWrapper(this.Exception);
                return _exceptionWrapper;
            }
            set
            {
                _exceptionWrapper = value;
            }
        }
    }
}
