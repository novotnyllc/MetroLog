
using System;
using System.Collections.Generic;
using System.Linq;

using MetroLog.Internal;

namespace MetroLog.Layouts
{
    public class PatternLayout : Layout
    {
        
        private static class Placeholders
        {
            public const string SequenceId = "%sequenceid";
            public const string Timestamp = "%timestamp";
            public const string Level = "%level";
            public const string ThreadId = "%thread";
            public const string Logger = "%logger";
            public const string Message = "%message";
            public const string Exception = "%exception";
            public const string NewLine = "%newline";

            public static IEnumerable<string> All
            {
                get
                {
                    yield return SequenceId;
                    yield return Timestamp;
                    yield return Level;
                    yield return ThreadId;
                    yield return Logger;
                    yield return Message;
                    yield return Exception;
                    yield return NewLine;
                }
            }
        }

        private const string DefaultPattern = "%sequenceid; %timestamp; %level; %logger; %message; %exception[EOL]";
        private readonly IList<IPatternExtension> patternExtensions = new List<IPatternExtension>();
        private string pattern;

        public PatternLayout()
            : this(DefaultPattern)
        {
        }

        public PatternLayout(string pattern)
        {
            this.Pattern = pattern ?? DefaultPattern;

            this.AddPatternExtension(new SequenceIdPatternExtension());
        }

        private class SequenceIdPatternExtension : IPatternExtension
        {
            private LogEventInfo logEventInfo;

            public string Placeholder
            {
                get
                {
                    return "%sequenceid";
                }
            }

            public string GetValue()
            {
                return this.logEventInfo.SequenceID.ToString();
            }

            public void SetLogEventInfo(LogEventInfo logEventInfo)
            {
                this.logEventInfo = logEventInfo;
            }
        }

        public string Pattern
        {
            get
            {
                return this.pattern;
            }
            set
            {
                this.pattern = value;
            }
        }

        public void AddPatternExtension(IPatternExtension patternExtension)
        {
            if (Placeholders.All.Contains(patternExtension.Placeholder) || this.patternExtensions.Contains(patternExtension)) // Eventually remove this check?  /// Contains should make sure that equals is implemented!!
            {
                InternalLogger.Current.Warn("PatternLayout cannot add placeholder [" + patternExtension.Placeholder + "] because this is a reserved key.");
            }
            else
            {
                this.patternExtensions.Add(patternExtension);
            }
        }

        public override string GetFormattedString(LogWriteContext context, LogEventInfo logEventInfo)
        {
            var value = this.Pattern;

            value = value.Replace(Placeholders.SequenceId, logEventInfo.SequenceID.ToString());
            value = value.Replace(Placeholders.Timestamp, logEventInfo.TimeStamp.LocalDateTime.ToString(LogManagerBase.DateTimeFormat));
            value = value.Replace(Placeholders.Level, logEventInfo.Level.ToString());
            value = value.Replace(Placeholders.ThreadId, Environment.CurrentManagedThreadId.ToString());
            value = value.Replace(Placeholders.Logger, logEventInfo.Logger);
            value = value.Replace(Placeholders.Message, logEventInfo.Message);
            value = value.Replace(Placeholders.Exception, logEventInfo.Exception != null ? logEventInfo.Exception.ToString() : string.Empty);
            value = value.Replace(Placeholders.NewLine, Environment.NewLine);

            foreach (var patternExtension in this.patternExtensions)
            {
                patternExtension.SetLogEventInfo(logEventInfo);
                value = value.Replace(patternExtension.Placeholder, patternExtension.GetValue());
            }

            return value;
        }
    }
}