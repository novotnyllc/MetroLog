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
    public class FileStreamingTarget : AsyncTarget
    {
        public FileStreamingTarget()
            : this(new SingleLineLayout())
        {
        }

        public FileStreamingTarget(Layout layout)
            : base(layout)
        {
        }

        protected override async Task<LogWriteOperation> WriteAsync(LogEventInfo entry)
        {
            var folder = await FileSnapshotTarget.EnsureInitializedAsync();

            // write...
            var filename = string.Format("Log - {0}.log", entry.TimeStamp.ToString("yyyyMMdd"));
            var file = await folder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
            await FileIO.AppendTextAsync(file, this.Layout.GetFormattedString(entry) + "\r\n");

            // return...
            return new LogWriteOperation(this, entry, true);
        }
    }
}
