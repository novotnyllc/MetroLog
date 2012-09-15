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
    public class FileStreamingTarget : FileTargetBase
    {
        public FileStreamingTarget()
            : this(new SingleLineLayout())
        {
        }

        public FileStreamingTarget(Layout layout)
            : base(layout)
        {
            this.FileNamingParameters.IncludeLevel = false;
            this.FileNamingParameters.IncludeLogger = false;
            this.FileNamingParameters.IncludeSequence = false;
            this.FileNamingParameters.IncludeSession = false;
            this.FileNamingParameters.IncludeTimestamp = FileTimestampMode.Date;
        }

        protected override async Task<LogWriteOperation> WriteAsync(LogWriteContext context, LogEventInfo entry)
        {
            var folder = await FileSnapshotTarget.EnsureInitializedAsync();

            // cleanup...
            await this.CheckCleanupAsync(folder);

            // write...
            var filename = this.FileNamingParameters.GetFilename(context, entry);
            var file = await folder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);

            // need to append a session header...

            // append...
            await FileIO.AppendTextAsync(file, this.Layout.GetFormattedString(context, entry) + "\r\n");

            // return...
            return new LogWriteOperation(this, entry, true);
        }
    }
}
