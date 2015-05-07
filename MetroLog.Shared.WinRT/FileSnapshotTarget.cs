using System;
using System.Threading.Tasks;

using Windows.Storage;

using MetroLog.Layouts;

namespace MetroLog.Targets
{
    public class FileSnapshotTarget : WinRTFileTarget
    {
        public FileSnapshotTarget()
            : this(new FileSnapshotLayout())
        {
        }

        public FileSnapshotTarget(Layout layout)
            : base(layout)
        {
            this.FileNamingParameters.IncludeLevel = true;
            this.FileNamingParameters.IncludeLogger = true;
            this.FileNamingParameters.IncludeSession = false;
            this.FileNamingParameters.IncludeSequence = true;
            this.FileNamingParameters.IncludeTimestamp = FileTimestampMode.DateTime;
            this.FileNamingParameters.CreationMode = FileCreationMode.ReplaceIfExisting;
        }

        protected override Task WriteTextToFileCore(IStorageFile file, string contents)
        {
            return FileIO.WriteTextAsync(file, contents).AsTask();
        }

        protected override void WriteTextToFile(IStorageFile file, string contents)
        {
            FileIO.WriteTextAsync(file, contents).GetResults();
        }
    }
}