using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebTargetSample.Json;

namespace WebTargetSample.Model
{
    public class JsonPostWrapper
    {
        [JsonConverter(typeof(LoggingEnvironmentConverter))]
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