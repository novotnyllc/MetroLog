using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Targets
{
    public class JsonPostWrapper
    {
        public ILoggingEnvironment Environment { get; set; }
        public LogEventInfo[] Events { get; set; }

        internal JsonPostWrapper(ILoggingEnvironment environment, IEnumerable<LogEventInfo> events)
        {
            this.Environment = environment;
            this.Events = events.ToArray();
        }

        internal string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
