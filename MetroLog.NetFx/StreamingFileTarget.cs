using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Layouts;
using MetroLog.Targets;

namespace MetroLog
{
    public class StreamingFileTarget : FileTarget
    {
        public StreamingFileTarget()
            : this(new SingleLineLayout())
        {
        }

        public StreamingFileTarget(Layout layout) : base(layout)
        {
            FileNamingParameters.IncludeLevel = false;
            FileNamingParameters.IncludeLogger = false;
            FileNamingParameters.IncludeSequence = false;
            FileNamingParameters.IncludeSession = false;
            FileNamingParameters.IncludeTimestamp = FileTimestampMode.Date;
            FileNamingParameters.CreationMode = FileCreationMode.AppendIfExisting;
        }


        protected override Task WriteTextToFileCore(StreamWriter file, string contents)
        {
            return file.WriteLineAsync(contents);
        }
    }
}
