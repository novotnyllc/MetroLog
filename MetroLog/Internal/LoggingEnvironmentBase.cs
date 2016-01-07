using System;
using System.Diagnostics;
using System.Reflection;

namespace MetroLog.Internal
{
    public abstract class LoggingEnvironmentBase : ILoggingEnvironment
    {
        public Guid SessionId { get; private set; }
        public string FxProfile { get; private set; }
        public bool IsDebugging { get; private set; }
        public Version MetroLogVersion { get; private set; }
        public string MachineName { get; protected set; }

        protected LoggingEnvironmentBase(string fxProfile)
        {
            this.SessionId = Guid.NewGuid();
            this.FxProfile = fxProfile;
            this.IsDebugging = Debugger.IsAttached;
            this.MetroLogVersion = typeof(ILogger).GetTypeInfo().Assembly.GetName().Version;
        }

        public string ToJson()
        {
            return SimpleJson.SerializeObject(this);
        }
    }
}
