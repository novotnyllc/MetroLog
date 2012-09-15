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
    public class FileSnapshotTarget : FileTargetBase
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
        }

        protected override async Task<LogWriteOperation> WriteAsync(LogWriteContext context, LogEventInfo entry)
        {
            var folder = await EnsureInitializedAsync();
            if (folder == null)
                return new LogWriteOperation(this, entry, false);

            // cleanup...
            await this.CheckCleanupAsync(folder);

            // create the file...
            var filename = this.FileNamingParameters.GetFilename(context, entry);
            var file = await folder.CreateFileAsync(filename).AsTask();
            
            // write...
            string buf = this.Layout.GetFormattedString(context, entry);
            await FileIO.WriteTextAsync(file, buf);

            // return...
            return new LogWriteOperation(this, entry, true);
        }
    }
}
