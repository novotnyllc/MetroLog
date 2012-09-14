using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTargetSample.Model
{
    public class JsonPostWrapper
    {
        public LoggingEnvironment Environment { get; set; }
        public LogEventInfo[] Events { get; set; }

        public JsonPostWrapper()
        {
        }

        public static JsonPostWrapper FromJson(string json)
        {
            return JsonConvert.DeserializeObject<JsonPostWrapper>(json);
        }
    }
}