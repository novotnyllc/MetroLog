using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Layouts;

namespace MetroLog.Targets
{
    public class SnapshotFileTarget : FileTarget
    {
        public SnapshotFileTarget()
            : this(new FileSnapshotLayout())
        {
        }

        public SnapshotFileTarget(Layout layout)
            : base(layout)
        {
            FileNamingParameters.IncludeLevel = true;
            FileNamingParameters.IncludeLogger = true;
            FileNamingParameters.IncludeSession = false;
            FileNamingParameters.IncludeSequence = true;
            FileNamingParameters.IncludeTimestamp = FileTimestampMode.DateTime;
            FileNamingParameters.CreationMode = FileCreationMode.ReplaceIfExisting;
        }

        protected override async Task WriteTextToFileCore(StreamWriter stream, string contents)
        {
            await stream.WriteAsync(contents);
        }
    }

    [Obsolete("Use SnapshotFileTarget")]
    public class FileSnapshotTarget : SnapshotFileTarget
    {
        public FileSnapshotTarget()
        {
        }

        public FileSnapshotTarget(Layout layout) : base(layout)
        {
        }
    }


}
