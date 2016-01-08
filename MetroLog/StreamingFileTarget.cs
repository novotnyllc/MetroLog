using MetroLog.Layouts;
using MetroLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace MetroLog.Targets
{
    /// <summary>
    /// Defines a target that will append messages to a single file.
    /// </summary>
    public class StreamingFileTarget : FileTarget
    {
        public StreamingFileTarget()
            : this(new SingleLineLayout())
        {
        }

        public StreamingFileTarget(Layout layout)
            : base(layout)
        {
        }

        protected override Task EnsureInitialized()
        {
            throw new NotImplementedException();
        }

        protected override Task DoCleanup(Regex pattern, DateTime threshold)
        {
            throw new NotImplementedException();
        }

        protected override Task<Stream> GetCompressedLogsInternal()
        {
            throw new NotImplementedException();
        }

        protected override Task<LogWriteOperation> DoWriteAsync(string fileName, string contents, LogEventInfo entry)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class FileTarget : FileTargetBase
    {
        protected FileTarget(Layout layout) : base(layout)
        {
        }
    }
}
