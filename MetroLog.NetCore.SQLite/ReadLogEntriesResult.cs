using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Targets
{
    public class ReadLogEntriesResult
    {
        public List<LogEventInfoItem> Events { get; private set; }
        public List<SessionHeaderItem> Headers { get; private set; }

        internal ReadLogEntriesResult(IEnumerable<LogEventInfoItem> events, IEnumerable<SessionHeaderItem> headers)
        {
            this.Events = new List<LogEventInfoItem>(events);
            this.Headers = new List<SessionHeaderItem>(headers);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
