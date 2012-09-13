using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    public class ILoggerEventArgs : EventArgs
    {
        public ILogger Logger { get; private set; }

        public ILoggerEventArgs(ILogger logger)
        {
            this.Logger = logger;
        }
    }
}
