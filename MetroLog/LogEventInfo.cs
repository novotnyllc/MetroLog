using System;
using System.Collections.Generic;
using System.Threading;

using MetroLog.Internal;

namespace MetroLog
{
    public class LogEventInfo
    {
        private ExceptionWrapper exceptionWrapper;
        private static long globalSequenceId;

        public LogEventInfo(LogLevel level, string logger, string message, Exception ex)
        {
            this.Level = level;
            this.Logger = logger;
            this.Message = message;
            this.Exception = ex;
            this.TimeStamp = LogManagerBase.GetDateTime();
            this.SequenceID = GetNextSequenceId();
        }

        internal static long GetNextSequenceId()
        {
            return Interlocked.Increment(ref globalSequenceId);
        }

        public string ToJson()
        {
            return SimpleJson.SerializeObject(this);
        }

        public long SequenceID { get; set; }

        public LogLevel Level { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        [JsonIgnore]
        public Exception Exception { get; set; }

        public ExceptionWrapper ExceptionWrapper
        {
            get
            {
                if (this.exceptionWrapper == null && this.Exception != null)
                {
                    this.exceptionWrapper = new ExceptionWrapper(this.Exception);
                }
                return this.exceptionWrapper;
            }
            set
            {
                this.exceptionWrapper = value;
            }
        }

		public IEnumerable<LogEventInfo> CrashRecords { get; set; }
    }
}