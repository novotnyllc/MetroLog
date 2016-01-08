using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MetroLog.Internal
{
    public abstract class LoggingEnvironmentBase : ILoggingEnvironment
    {
        public Guid SessionId { get; private set; }
        public string FxProfile { get; private set; }
        public bool IsDebugging { get; private set; }
        public Version MetroLogVersion { get; private set; }

        protected LoggingEnvironmentBase(string fxProfile)
        {
            // common...
            SessionId = Guid.NewGuid();
            FxProfile = fxProfile;
            IsDebugging = Debugger.IsAttached;
            MetroLogVersion = typeof(ILogger).GetTypeInfo().Assembly.GetName().Version;
        }

        public string ToJson()
        {
            return SimpleJson.SerializeObject(this);
        }
    }
}
