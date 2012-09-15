using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MetroLog.Targets
{
    /// <summary>
    /// Defines a class that allows the user to configure file naming.
    /// </summary>
    public class FileNamingParameters
    {
        public bool IncludeLevel { get; set; }
        public FileTimestampMode IncludeTimestamp { get; set; }
        public bool IncludeLogger { get; set; }
        public bool IncludeSession { get; set; }
        public bool IncludeSequence { get; set; }

        public FileNamingParameters()
        {
            this.IncludeLevel = false;
            this.IncludeTimestamp = FileTimestampMode.Date;
            this.IncludeLogger = false;
            this.IncludeSession = true;
            this.IncludeSequence = false;
        }

        public string GetFilename(LogWriteContext context, LogEventInfo entry)
        {
            var builder = new StringBuilder();
            builder.Append("Log");
            if (this.IncludeLevel)
            {
                builder.Append(" - ");
                builder.Append(entry.Level.ToString().ToUpper());
            }
            if (this.IncludeLogger)
            {
                builder.Append(" - ");
                builder.Append(entry.Logger);
            }
            if (this.IncludeTimestamp != FileTimestampMode.None)
            {
                bool date = ((int)this.IncludeTimestamp & (int)FileTimestampMode.Date) != 0;
                if (date)
                {
                    builder.Append(" - ");
                    builder.Append(entry.TimeStamp.ToString("yyyyMMdd"));
                }

                bool time = ((int)this.IncludeTimestamp & (int)FileTimestampMode.Time) != 0;
                if(time)
                {
                    if(date)
                        builder.Append(" ");
                    else
                        builder.Append(" - ");
                    builder.Append(entry.TimeStamp.ToString("HHmmss"));
                }
            }
            if (this.IncludeSession)
            {
                builder.Append(" - ");
                builder.Append(context.Environment.SessionId);
            }
            if (this.IncludeSequence)
            {
                builder.Append(" - ");
                builder.Append(entry.SequenceID);
            }

            // return...
            builder.Append(".log");
            return builder.ToString();
        }

        public Regex GetRegex()
        {
            var builder = new StringBuilder();
            builder.Append("^Log");

            // stuff...
            if (this.IncludeLevel)
            {
                builder.Append(@"\s*-\s*");
                builder.Append(@"\w+");
            }
            if (this.IncludeLogger)
            {
                builder.Append(@"\s*-\s*");
                builder.Append(@"[\w\s]+");
            }
            if (this.IncludeTimestamp != FileTimestampMode.None)
            {
                bool date = ((int)this.IncludeTimestamp & (int)FileTimestampMode.Date) != 0;
                if (date)
                {
                    builder.Append(@"\s*-\s*");
                    builder.Append("[0-9]{8}");
                }

                bool time = ((int)this.IncludeTimestamp & (int)FileTimestampMode.Time) != 0;
                if (time)
                {
                    if (date)
                        builder.Append(@"\s+");
                    else
                        builder.Append(@"\s*-\s*");
                    builder.Append("[0-9]{6}");
                }
            }
            if (this.IncludeSession)
            {
                builder.Append(@"\s*-\s*");
                builder.Append(@"[a-fA-F0-9\-]+");
            }
            if (this.IncludeSequence)
            {
                builder.Append(@"\s*-\s*");
                builder.Append("[0-9]+");
            }

            // log...
            builder.Append(".log$");

            // go...
            var regex = new Regex(builder.ToString(), RegexOptions.Singleline | RegexOptions.IgnoreCase);
            return regex;
        }
    }
}
