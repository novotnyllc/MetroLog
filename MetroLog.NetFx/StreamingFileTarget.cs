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

        public StreamingFileTarget(Layout layout) : base(layout)
        {
            FileNamingParameters.IncludeLevel = false;
            FileNamingParameters.IncludeLogger = false;
            FileNamingParameters.IncludeSequence = false;
            FileNamingParameters.IncludeSession = false;
            FileNamingParameters.IncludeTimestamp = FileTimestampMode.Date;
            FileNamingParameters.CreationMode = FileCreationMode.AppendIfExisting;
        }

        protected override void WriteTextToFile(StreamWriter file, string contents)
        {
            file.WriteLine(contents);
        }

        protected override Task WriteTextToFileCore(StreamWriter file, string contents)
        {
            return file.WriteLineAsync(contents);
        }
    }
}
