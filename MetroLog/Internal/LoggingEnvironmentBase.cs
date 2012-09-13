using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Internal
{
    public abstract class LoggingEnvironmentBase : ILoggingEnvironment
    {
        public Dictionary<String, object> Values { get; private set; }

        protected LoggingEnvironmentBase(string fxProfile)
        {
            this.Values = new Dictionary<string, object>();

            // common.
            this.Values["FxProfile"] = fxProfile;
            this.Values["IsDebugging"] = Debugger.IsAttached;
        }

        public abstract string ToJson();
    }
}
