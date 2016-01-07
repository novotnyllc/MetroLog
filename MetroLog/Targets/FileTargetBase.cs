using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using MetroLog.Internal;
using MetroLog.Layouts;

namespace MetroLog.Targets
{
    /// <summary>
    ///     Base class for file targets.
    /// </summary>
    public abstract class FileTargetBase : AsyncTarget
    {
        /// <summary>
        ///     Gets an object that defines the file naming parameters.
        /// </summary>
        public FileNamingParameters FileNamingParameters { get; private set; }

        /// <summary>
        ///     Gets or sets the number of days to retain log files for.
        /// </summary>
        public int RetainDays { get; set; }

        protected const string LogFolderName = "MetroLogs";

        /// <summary>
        ///     Holds the next cleanup time.
        /// </summary>
        protected DateTime NextCleanupUtc { get; set; }

        private readonly AsyncLock _lock = new AsyncLock();

        protected FileTargetBase(Layout layout)
            : base(layout)
        {
            this.FileNamingParameters = new FileNamingParameters();
            this.RetainDays = 30;
        }

        protected abstract Task EnsureInitialized();

        protected abstract Task DoCleanup(Regex pattern, DateTime threshold);

        protected abstract Task<Stream> GetCompressedLogsInternal();

        internal Task<Stream> GetCompressedLogs()
        {
            return this.GetCompressedLogsInternal();
        }

        internal async Task ForceCleanupAsync()
        {
            // threshold...
            var threshold = DateTime.UtcNow.AddDays(0 - this.RetainDays);

            // walk...
            var regex = this.FileNamingParameters.GetRegex();

            await this.DoCleanup(regex, threshold);
        }

        private async Task CheckCleanupAsync()
        {
            var now = DateTime.UtcNow;
            if (now < this.NextCleanupUtc || this.RetainDays < 1)
            {
                return;
            }

            try
            {
                // threshold...
                var threshold = now.AddDays(0 - this.RetainDays);

                // walk...
                var regex = this.FileNamingParameters.GetRegex();

                await this.DoCleanup(regex, threshold);
            }
            finally
            {
                // reset...
                this.NextCleanupUtc = DateTime.UtcNow.AddHours(1);
            }
        }

        protected override sealed async Task<LogWriteOperation> WriteAsyncCore(LogWriteContext context, LogEventInfo entry)
        {
            if (context.IsFatalException) // TODO Refactor duplicated code
            {
                await this.EnsureInitialized();
                await this.CheckCleanupAsync();

                var filename = this.FileNamingParameters.GetFilename(context, entry);
                var contents = this.Layout.GetFormattedString(context, entry);

                return this.DoWrite(filename, contents, entry);
            }
            else
            {
                using (await this._lock.LockAsync())
                {
                    await this.EnsureInitialized();
                    await this.CheckCleanupAsync();

                    var filename = this.FileNamingParameters.GetFilename(context, entry);
                    var contents = this.Layout.GetFormattedString(context, entry);

                    return await this.DoWriteAsync(filename, contents, entry);
                }
            }
        }

        protected abstract Task<LogWriteOperation> DoWriteAsync(string fileName, string contents, LogEventInfo entry);

        protected abstract LogWriteOperation DoWrite(string fileName, string contents, LogEventInfo entry);
    }
}