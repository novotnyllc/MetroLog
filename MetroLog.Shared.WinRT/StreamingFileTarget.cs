using MetroLog.Layouts;
using MetroLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MetroLog.Targets
{
    /// <summary>
    /// Defines a target that will append messages to a single file.
    /// </summary>
    public class StreamingFileTarget : WinRTFileTarget
    {
        public StreamingFileTarget()
            : this(new SingleLineLayout())
        {
        }

        public StreamingFileTarget(Layout layout)
            : base(layout)
        {
            this.FileNamingParameters.IncludeLevel = false;
            this.FileNamingParameters.IncludeLogger = false;
            this.FileNamingParameters.IncludeSequence = false;
            this.FileNamingParameters.IncludeSession = false;
            this.FileNamingParameters.IncludeTimestamp = FileTimestampMode.Date;
            FileNamingParameters.CreationMode = FileCreationMode.AppendIfExisting;
        }

        protected override Task WriteTextToFileCore(IStorageFile file, string contents)
        {
            return FileIO.AppendTextAsync(file, contents + Environment.NewLine).AsTask();
        }
    }

    [Obsolete("Use StreamingFileTarget")]
    public class FileStreamingTarget : StreamingFileTarget
    {
        public FileStreamingTarget()
        {
        }

        public FileStreamingTarget(Layout layout) : base(layout)
        {
        }
    }
}
