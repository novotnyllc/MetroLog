using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetroLog.Layouts;
using Windows.Storage;

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
            FileNamingParameters.CreationMode = FileCreationMode.ReplaceIfExisting;
        }

        protected override Task WriteTextToFileCore(IStorageFile file, string contents)
        {
            return FileIO.WriteTextAsync(file, contents).AsTask();
        }
    }
}
