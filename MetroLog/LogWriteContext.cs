using MetroLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    public class LogWriteContext
    {
        private static ILoggingEnvironment _environment;

        public LogWriteContext()
        {
        }

        static LogWriteContext()
        {
            _environment = PlatformAdapter.Resolve<ILoggingEnvironment>();
        }

        public ILoggingEnvironment Environment
        {
            get
            {
                return _environment;
            }
        }
    }
}
