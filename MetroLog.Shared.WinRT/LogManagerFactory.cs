using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    class LogManagerFactory : ILogManagerCreator
    {
        public ILogManager Create(LoggingConfiguration configuration)
        {
            return new LogManager(configuration);
        }
    }
}
