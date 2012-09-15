using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTargetSample.Model
{
    public class LogEventInfo
    {
        public long SequenceID { get; set; }
        public LogLevel Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public ExceptionWrapper Exception { get; set; }
    }
}