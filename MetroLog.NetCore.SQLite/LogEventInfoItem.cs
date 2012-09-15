using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Targets
{
    /// <summary>
    /// Defines the class uses for serializing log entries to the database.
    /// </summary>
    public class LogEventInfoItem
    {
        [PrimaryKey, AutoIncrement]
        public int ItemId { get; set; }

        public DateTime DateTimeUtc { get; set; }
        public string SessionId { get; set; }
        public long SequenceId { get; set; }
        public LogLevel Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }

        public bool HasException { get; set; }
        public string Exception { get; set; }
        public int ExceptionHresult { get; set; }

        public LogEventInfoItem()
        {
        }

        internal static LogEventInfoItem GetForInsert(LogWriteContext context, LogEventInfo info)
        {
            var item = new LogEventInfoItem()
            {
                SessionId = context.Manager.LoggingEnvironment.SessionId.ToString(),
                DateTimeUtc = info.TimeStamp.UtcDateTime,
                SequenceId = info.SequenceID,
                Level = info.Level,
                Logger = info.Logger,
                Message = info.Message
            };

            // if...
            if (info.Exception != null)
                item.SetException(info.Exception);

            // return...
            return item;
        }

        private void SetException(Exception ex)
        {
            this.HasException = true;
            this.Exception = ex.ToString();
            this.ExceptionHresult = ex.HResult;
        }
    }
}
