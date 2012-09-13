using MetroLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    public class LoggingEnvironment : LoggingEnvironmentBase
    {
        public LoggingEnvironment()
            : base(Environment.Version.ToString())
        {
            this.Values["MachineName"] = Environment.MachineName;
        }

        public override string ToJson()
        {
            throw new NotImplementedException();
        }
    }
}
