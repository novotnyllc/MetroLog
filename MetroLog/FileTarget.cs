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


    public abstract class FileTarget : FileTargetBase
    {
        protected FileTarget(Layout layout) : base(layout)
        {
        }

        protected override Task EnsureInitialized()
        {
            throw new NotImplementedException();
        }

        protected sealed override Task DoCleanup(Regex pattern, DateTime threshold)
        {
            throw new NotImplementedException();
        }

        protected override Task<Stream> GetCompressedLogsInternal()
        {
            throw new NotImplementedException();
        }

        protected sealed override Task<LogWriteOperation> DoWriteAsync(StreamWriter streamWriter, string contents, LogEventInfo entry)
        {
            throw new NotImplementedException();
        }

        protected abstract Task WriteTextToFileCore(StreamWriter file, string contents);

        protected override Task<Stream> GetWritableStreamForFile(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
