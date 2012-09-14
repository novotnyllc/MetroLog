using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetroLog.Internal;
using Newtonsoft.Json;

namespace MetroLog
{
    public class LogEventInfo
    {
        public long SequenceID { get; private set; }
        public LogLevel Level { get; private set; }
        public string Logger { get; private set; }
        public string Message { get; private set; }
        public DateTimeOffset TimeStamp { get; private set; }

       // [JsonConverter(typeof(ExceptionConverter))]
        public Exception Exception { get; private set; }

        private static long _globalSequenceId;

        internal LogEventInfo(LogLevel level, string logger, string message, Exception ex)
        {
            Level = level;
            Logger = logger;
            Message = message;
            Exception = ex;
            TimeStamp = LogManager.GetDateTime();
            SequenceID = Interlocked.Increment(ref _globalSequenceId);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
