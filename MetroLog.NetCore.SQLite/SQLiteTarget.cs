using MetroLog.Layouts;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

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

        /// <summary>
        /// Holds session headers.
        /// </summary>
        private static Dictionary<Guid, SessionHeaderItem> Headers { get; set; }
        private static object _headersLock = new object();

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

        static SQLiteTarget()
        {
            Headers = new Dictionary<Guid, SessionHeaderItem>();
        }

        protected override async Task<LogWriteOperation> WriteAsync(LogWriteContext context, LogEventInfo entry)
        {
            await EnsureInitialize();

            // cleanup
            await CheckCleanup();

            // ok...
            var conn = GetConnection();
            var session = await GetSessionAsync(context.Environment);
            await conn.InsertAsync(LogEventInfoItem.GetForInsert(context, entry, session));

            // return...
            return new LogWriteOperation(this, entry, true);
        }

        private async Task<SessionHeaderItem> GetSessionAsync(ILoggingEnvironment environment)
        {
            // check...
            lock (_headersLock)
            {
                if (Headers.ContainsKey(environment.SessionId))
                    return Headers[environment.SessionId];
            }

            var conn = GetConnection();
            var header = await conn.Table<SessionHeaderItem>().Where(v => v.SessionGuid == environment.SessionId).FirstOrDefaultAsync();
            if(header == null)
            {
                header = SessionHeaderItem.CreateForEnvironment(environment);
                await conn.InsertAsync(header);
            }

            // set...
            lock (_headersLock)
            {
                if (!(Headers.ContainsKey(environment.SessionId)))
                    Headers[environment.SessionId] = header;
            }

            // return...
            return header;
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
            // check the folder...
            if (this.DatabasePath.Contains("\\") || this.DatabasePath.Contains("/"))
            {
                var folder = Path.GetDirectoryName(this.DatabasePath);
                try
                {
                    await ApplicationData.Current.LocalFolder.CreateFolderAsync(folder);
                }
                catch
                {
                    // ignore...
                }
            }

            // get...
            var conn = GetConnection();
            await conn.CreateTablesAsync<LogEventInfoItem, SessionHeaderItem>();
        }

        private SQLiteAsyncConnection GetConnection()
        {
            return new SQLiteAsyncConnection(this.DatabasePath);
        }

        public async Task<ReadLogEntriesResult> ReadLogEntriesAsync(LogReadQuery query)
        {
            var builder = new StringBuilder();
            var args = new List<object>();
            builder.Append("select * from logeventinfoitem where level in (");
            bool first = true;
            if (query.IsTraceEnabled)
                AppendLevel(builder, args, LogLevel.Trace, ref first);
            if (query.IsDebugEnabled)
                AppendLevel(builder, args, LogLevel.Debug, ref first);
            if (query.IsInfoEnabled)
                AppendLevel(builder, args, LogLevel.Info, ref first);
            if (query.IsWarnEnabled)
                AppendLevel(builder, args, LogLevel.Warn, ref first);
            if (query.IsErrorEnabled)
                AppendLevel(builder, args, LogLevel.Error, ref first);
            if (query.IsFatalEnabled)
                AppendLevel(builder, args, LogLevel.Fatal, ref first);
            builder.Append(")");
            if (query.FromDateTimeUtc != DateTime.MinValue)
            {
                builder.Append(" and datetimeutc >= ?");
                args.Add(query.FromDateTimeUtc);
            }
            builder.Append(" order by itemid desc");
            if (query.Top > 0)
            {
                builder.Append(" limit ");
                builder.Append(query.Top);
            }

            // create...
            var conn = this.GetConnection();
            var events = (await conn.QueryAsync<LogEventInfoItem>(builder.ToString(), args.ToArray())).ToList();

            // get the unique sessions...
            var sessionIds = new List<int>();
            foreach (var theEvent in events)
            {
                if (!(sessionIds.Contains(theEvent.SessionId)))
                    sessionIds.Add(theEvent.SessionId);
            }

            // load...
            var headers = await SessionHeaderItem.GetByIdsAsync(conn, sessionIds);
            return new ReadLogEntriesResult(events, headers);
        }

        public async Task<IStorageFile> PackageToTempFileAsync(LogReadQuery query, int maxPayloadMb = 5 * (1024 * 1024))
        {
            var result = await ReadLogEntriesAsync(query);
            return await PackageToTempFileAsync(result, maxPayloadMb);
        }

        private async Task<IStorageFile> PackageToTempFileAsync(ReadLogEntriesResult result, int maxPayloadMb = 5 * (1024 * 1024))
        {
            var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(Guid.NewGuid().ToString() + ".txt");

            // TODO - implement payload maximum...

            // ok...
            var json = result.ToJson();
            await FileIO.WriteTextAsync(file, json);

            // return...
            return file;
        }

        private void AppendLevel(StringBuilder builder, List<object> args, LogLevel level, ref bool first)
        {
            if (first)
                first = false;
            else
                builder.Append(",");
            builder.Append("?");
            args.Add(level);
        }
    }
}
