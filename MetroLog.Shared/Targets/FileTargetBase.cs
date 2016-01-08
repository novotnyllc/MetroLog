using System.Text.RegularExpressions;
using MetroLog.Internal;
using MetroLog.Layouts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MetroLog.Targets
{
    /// <summary>
    /// Base class for file targets.
    /// </summary>
    public abstract class FileTargetBase : AsyncTarget
    {
        /// <summary>
        /// Gets an object that defines the file naming parameters.
        /// </summary>
        public FileNamingParameters FileNamingParameters { get; private set; }

        /// <summary>
        /// Gets or sets the number of days to retain log files for.
        /// </summary>
        public int RetainDays { get; set; }

        protected const string LogFolderName = "MetroLogs";

        /// <summary>
        /// Holds the next cleanup time.
        /// </summary>
        protected DateTime NextCleanupUtc { get; set; }

        readonly AsyncLock _lock = new AsyncLock();

        protected FileTargetBase(Layout layout)
            : base(layout)
        {
            FileNamingParameters = new FileNamingParameters();
            RetainDays = 30;
        }

        protected abstract Task EnsureInitialized();
        protected abstract Task DoCleanup(Regex pattern, DateTime threshold);
        protected abstract Task<Stream> GetCompressedLogsInternal();

        internal Task<Stream> GetCompressedLogs()
        {
            return GetCompressedLogsInternal();
        }

        internal async Task ForceCleanupAsync()
        {
            // threshold...
            var threshold = DateTime.UtcNow.AddDays(0 - RetainDays);

            // walk...
            var regex = FileNamingParameters.GetRegex();

            await DoCleanup(regex, threshold);
        }

        async Task CheckCleanupAsync()
        {
            var now = DateTime.UtcNow;
            if (now < NextCleanupUtc || RetainDays < 1)
                return;

            try
            {
                // threshold...
                var threshold = now.AddDays(0 - RetainDays);

                // walk...
                var regex = FileNamingParameters.GetRegex();

                await DoCleanup(regex, threshold);
            }
            finally
            {
                // reset...
                NextCleanupUtc = DateTime.UtcNow.AddHours(1);
            }
        }


        sealed protected override async Task<LogWriteOperation> WriteAsyncCore(LogWriteContext context, LogEventInfo entry)
        {
            using (await _lock.LockAsync())
            {
                await EnsureInitialized();
                await CheckCleanupAsync();

                var filename = FileNamingParameters.GetFilename(context, entry);
                var contents = Layout.GetFormattedString(context, entry);

                return await DoWriteAsync(filename, contents, entry);
            }
        }

        protected abstract Task<LogWriteOperation> DoWriteAsync(string fileName, string contents, LogEventInfo entry);
    }
}
