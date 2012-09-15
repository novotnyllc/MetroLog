using MetroLog.Layouts;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Targets
{
    /// <summary>
    /// Defines a target that is able to write to a SQLite database.
    /// </summary>
    public class SQLiteTarget : AsyncTarget
    {
        /// <summary>
        /// Gets or sets the numbers of days that log entries should be retained for.
        /// </summary>
        public int RetainDays { get; set; }

        /// <summary>
        /// Holds the next cleanup time.
        /// </summary>
        private DateTime NextCleanupUtc { get; set; }

        /// <summary>
        /// Gets or sets the database path.
        /// </summary>
        public string DatabasePath { get; set; }

        /// <summary>
        /// Holds whether the target has been initialized.
        /// </summary>
        private bool IsInitialized { get; set; }

        public SQLiteTarget()
            : this(new NullLayout())
        {
        }

        public SQLiteTarget(Layout layout)
            : base(layout)
        {
            // defaults...
            this.RetainDays = 30;
            this.DatabasePath = "MetroLogs/MetroLog.db";
        }

        protected override async Task<LogWriteOperation> WriteAsync(LogWriteContext context, LogEventInfo entry)
        {
            await EnsureInitialize();

            // cleanup
            await CheckCleanup();

            // ok...
            var conn = GetConnection();
            await conn.InsertAsync(LogEventInfoItem.GetForInsert(context, entry));

            // return...
            return new LogWriteOperation(this, entry, true);
        }

        private async Task CheckCleanup()
        {
            if (DateTime.UtcNow > this.NextCleanupUtc && this.RetainDays > 0)
            {
                // delete out...
                try
                {
                    var threshold = DateTime.UtcNow.AddDays(0 - this.RetainDays);

                    // delete...
                    var conn = GetConnection();
                    await conn.ExecuteAsync("delete from LogEventInfoItem where datetimeutc <= ?", threshold);
                }
                catch (Exception ex)
                {
                    InternalLogger.Current.Error("Failed to run cleanup operation.", ex);
                }
            }
        }

        private async Task EnsureInitialize()
        {
            var conn = GetConnection();
            await conn.CreateTableAsync<LogEventInfoItem>();
        }

        private SQLiteAsyncConnection GetConnection()
        {
            return new SQLiteAsyncConnection(this.DatabasePath);
        }
    }
}
