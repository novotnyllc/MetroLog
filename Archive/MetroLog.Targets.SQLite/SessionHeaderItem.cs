using MetroLog.NetCore.Targets.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Targets
{
    public class SessionHeaderItem
    {
        [PrimaryKey, AutoIncrement]
        public int SessionId { get; set; }

        [Unique]
        public Guid SessionGuid { get; set; }

        public string Data { get; set; }

        public SessionHeaderItem()
        {
        }

        internal static SessionHeaderItem CreateForEnvironment(ILoggingEnvironment environment)
        {
            return new SessionHeaderItem() 
            {
                SessionGuid = environment.SessionId,
                Data = environment.ToJson()
            };
        }

        internal static async Task<IEnumerable<SessionHeaderItem>> GetByIdsAsync(SQLiteAsyncConnection conn, List<int> ids)
        {
            var results = new List<SessionHeaderItem>();
            foreach (var id in ids)
            {
                var session = await conn.FindAsync<SessionHeaderItem>(id);
                if (session != null)
                    results.Add(session);
            }

            // return...
            return results;
        }
    }
}
