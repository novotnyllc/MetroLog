using System.IO;
using System.Threading.Tasks;

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
            this.FileNamingParameters.CreationMode = FileCreationMode.AppendIfExisting;
        }

        protected override Task WriteTextToFileCore(StreamWriter file, string contents)
        {
            return file.WriteLineAsync(contents);
        }
    }
}