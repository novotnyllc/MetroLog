using System;
using System.Threading.Tasks;

using Windows.Storage;

using MetroLog.Layouts;

namespace MetroLog.Targets
{
    /// <summary>
    ///     Defines a target that will append messages to a single file.
    /// </summary>
    public class FileStreamingTarget : WinRTFileTarget
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
            this.FileNamingParameters.CreationMode = FileCreationMode.AppendIfExisting;
        }

        protected override Task WriteTextToFileCore(IStorageFile file, string contents)
        {
            return FileIO.AppendTextAsync(file, contents + Environment.NewLine).AsTask();
        }

        protected override void WriteTextToFile(IStorageFile file, string contents)
        {
            FileIO.AppendTextAsync(file, contents + Environment.NewLine).GetResults();
        }
    }
}