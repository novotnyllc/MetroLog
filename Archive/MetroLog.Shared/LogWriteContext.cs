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
        static ILoggingEnvironment _environment;

        public LogWriteContext()
        {
        }

        static LogWriteContext()
        {
            _environment = new LoggingEnvironment();
        }

        public ILoggingEnvironment Environment => _environment;
    }
}
