using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    public class LoggerEventArgs : EventArgs
    {
        public ILogger Logger { get; private set; }

        public LoggerEventArgs(ILogger logger)
        {
            Logger = logger;
        }
    }
}
