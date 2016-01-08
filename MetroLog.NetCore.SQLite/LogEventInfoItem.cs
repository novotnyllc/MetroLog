using MetroLog.NetCore.Targets.SQLite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Internal;


namespace MetroLog.Targets
{
    /// <summary>
    /// Defines the class uses for serializing log entries to the database.
    /// </summary>
    [JsonConverter(typeof(LogEventInfoItemConverter))]
    public class LogEventInfoItem
    {
        [PrimaryKey, AutoIncrement]
        public int ItemId { get; set; }

        public int SessionId { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public long SequenceId { get; set; }
        public LogLevel Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }

        public bool HasException { get; set; }
        public string Exception { get; set; }
        public string ExceptionTypeName { get; set; }
        public int ExceptionHresult { get; set; }

        public LogEventInfoItem()
        {
        }

        internal static LogEventInfoItem GetForInsert(LogWriteContext context, LogEventInfo info, SessionHeaderItem session)
        {
            var item = new LogEventInfoItem()
            {
                SessionId = session.SessionId,
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

        void SetException(Exception ex)
        {
            this.HasException = true;
            this.Exception = ex.ToString();
            this.ExceptionTypeName = ex.GetType().AssemblyQualifiedName;
            this.ExceptionHresult = ex.HResult;
        }

        internal string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        internal ExceptionWrapper GetExceptionWrapper()
        {
            if (this.HasException)
            {
                return new ExceptionWrapper()
                {
                    AsString = this.Exception,
                    TypeName = this.ExceptionTypeName,
                    Hresult = this.ExceptionHresult
                };
            }
            else
                return null;
        }
    }
}
