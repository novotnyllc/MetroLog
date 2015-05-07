
using System.Collections.Generic;
using System.Linq;
using MetroLog.Internal;


namespace MetroLog.Targets
{
    public class JsonPostWrapper
    {
        public ILoggingEnvironment Environment { get; set; }
        public LogEventInfo[] Events { get; set; }

        public JsonPostWrapper(ILoggingEnvironment environment, IEnumerable<LogEventInfo> events)
        {
            this.Environment = environment;
            this.Events = events.ToArray();
        }

        public string ToJson()
        {
            return SimpleJson.SerializeObject(this);
        }
    }
}
