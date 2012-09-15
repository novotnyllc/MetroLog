using MetroLog.Layouts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

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

        public static StorageFolder _logFolder = null;
        private const string LogFolderName = "MetroLogs";

        /// <summary>
        /// Holds the next cleanup time.
        /// </summary>
        private DateTime NextCleanupUtc { get; set; }

        protected FileTargetBase(Layout layout)
            : base(layout)
        {
            this.FileNamingParameters = new FileNamingParameters();
            this.RetainDays = 30;
        }

        public static async Task<StorageFolder> EnsureInitializedAsync()
        {
            var folder = _logFolder;
            if (folder == null)
            {
                StorageFolder logFolder = null;
                var root = ApplicationData.Current.LocalFolder;
                try
                {
                    logFolder = await root.GetFolderAsync(LogFolderName);
                }
                catch (FileNotFoundException ex)
                {
                    SinkException(ex);
                }

                // if...
                if (logFolder == null)
                {
                    try
                    {
                        logFolder = await root.CreateFolderAsync(LogFolderName, CreationCollisionOption.OpenIfExists);
                    }
                    catch (Exception ex)
                    {
                        SinkException(ex);
                    }

                    // if we get into trouble here, try and load it again... (something else should have created it)...
                    if (logFolder == null)
                        logFolder = await root.GetFolderAsync(LogFolderName);
                }

                // store it - but only if we have one...
                if (logFolder != null)
                    Interlocked.CompareExchange<StorageFolder>(ref _logFolder, logFolder, null);
            }
            return _logFolder;
        }

        private static void SinkException(Exception ex)
        {
            // no-op - just preventing compile warnings...
        }

        /// <summary>
        /// Cleans up any old log files.
        /// </summary>
        /// <returns></returns>
        protected async Task CheckCleanupAsync(StorageFolder folder)
        {
            var now = DateTime.UtcNow;
            if (now < this.NextCleanupUtc || this.RetainDays < 1)
                return;

            try
            {
                // threshold...
                var threshold = now.AddDays(0 - this.RetainDays);

                // walk...
                var regex = this.FileNamingParameters.GetRegex();
                var toDelete = new List<StorageFile>();
                foreach (var file in await folder.GetFilesAsync())
                {
                    if (regex.Match(file.Name).Success && file.DateCreated <= threshold)
                        toDelete.Add(file);
                }

                // walk...
                foreach (var file in toDelete)
                {
                    try
                    {
                        await file.DeleteAsync();
                    }
                    catch (Exception ex)
                    {
                        InternalLogger.Current.Warn(string.Format("Failed to delete '{0}'.", file.Path), ex);
                    }
                }
            }
            finally
            {
                // reset...
                this.NextCleanupUtc = DateTime.UtcNow.AddHours(1);
            }
        }
    }
}
