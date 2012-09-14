using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MetroLog.Internal
{
    public abstract class LoggingEnvironmentBase : ILoggingEnvironment
    {
        public string FxProfile { get; private set; }
        public bool IsDebugging { get; private set; }

        [JsonConverter(typeof(VersionConverter))]
        public Version MetroLogVersion { get; private set; }

        protected LoggingEnvironmentBase(string fxProfile)
        {
            // common...
            this.FxProfile = fxProfile;
            this.IsDebugging = Debugger.IsAttached;
            this.MetroLogVersion = typeof(ILogger).GetTypeInfo().Assembly.GetName().Version;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
