using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    public class LogWriteContext
    {
        public ILogManager Manager { get; private set; }

        internal LogWriteContext(ILogManager manager)
        {
            this.Manager = manager;
        }

        public ILoggingEnvironment Environment
        {
            get
            {
                return Manager.LoggingEnvironment;
            }
        }
    }
}
