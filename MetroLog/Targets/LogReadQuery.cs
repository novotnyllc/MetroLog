using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Targets
{
    public class LogReadQuery
    {
        public bool IsTraceEnabled { get; set; }
        public bool IsDebugEnabled { get; set; }
        public bool IsInfoEnabled { get; set; }
        public bool IsWarnEnabled { get; set; }
        public bool IsErrorEnabled { get; set; }
        public bool IsFatalEnabled { get; set; }

        /// <summary>
        /// Gets or sets the number of items to read.
        /// </summary>
        /// <remarks>By default this is set to <c>1000</c>. Set to <c>0</c> to remove any limit.</remarks>
        public int Top { get; set; }

        /// <summary>
        /// Gets or sets the earliest date/time to read.
        /// </summary>
        /// <remarks>By default this is <c>DateTime.UtcNow.AddDays(-7)</c>. Set to <c>DateTime.MinValue</c> to remove this constraint.</remarks>
        public DateTime FromDateTimeUtc { get; set; }

        public LogReadQuery()
        {
            this.IsTraceEnabled = false;
            this.IsDebugEnabled = false;
            this.IsInfoEnabled = true;
            this.IsWarnEnabled = true;
            this.IsErrorEnabled = true;
            this.IsFatalEnabled = true;

            this.Top = 1000;

            this.FromDateTimeUtc = DateTime.UtcNow.AddDays(-7);
        }

        public void SetLevels(LogLevel from, LogLevel to)
        {
            this.IsTraceEnabled = LogLevel.Trace >= from && LogLevel.Trace <= to;
            this.IsDebugEnabled = LogLevel.Debug >= from && LogLevel.Debug <= to;
            this.IsInfoEnabled = LogLevel.Info >= from && LogLevel.Info <= to;
            this.IsWarnEnabled = LogLevel.Warn >= from && LogLevel.Warn <= to;
            this.IsErrorEnabled = LogLevel.Error >= from && LogLevel.Error <= to;
            this.IsFatalEnabled = LogLevel.Fatal >= from && LogLevel.Fatal <= to;
        }
    }
}
