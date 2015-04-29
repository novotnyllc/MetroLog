using System.IO;
using System.Threading.Tasks;
using MetroLog.Layouts;
using Windows.Storage;

namespace MetroLog.Targets
{
    public class StreamingFileTarget : Wp8FileTarget
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

        protected override async Task WriteTextToFileCore(IStorageFile file, string contents)
        {
            using (var sw = new StreamWriter(file.Path, true))
            {
                await sw.WriteLineAsync(contents);
            }
        }
    }
}