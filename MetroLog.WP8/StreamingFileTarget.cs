using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using MetroLog.Layouts;

namespace MetroLog.Targets
{
    public class StreamingFileTarget : FileTarget
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

        protected override async Task WriteTextToFileCore(IStorageFile file, string contents)
        {
            using (var sw = new StreamWriter(file.Path, true))
            {
                await sw.WriteLineAsync(contents);
            }
        }
    }

    [Obsolete("Use StreamingFileTarget instead")]
    public class Wp8FileStreamingTarget : StreamingFileTarget
    {
        public Wp8FileStreamingTarget()
        {
        }

        public Wp8FileStreamingTarget(Layout layout) : base(layout)
        {
        }
    }
}
