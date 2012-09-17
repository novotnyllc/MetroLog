using MetroLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    public interface ILogManager
    {
        LoggingConfiguration DefaultConfiguration { get; }

        ILogger GetLogger<T>(LoggingConfiguration config = null);
        ILogger GetLogger(Type type, LoggingConfiguration config = null);
        ILogger GetLogger(string name, LoggingConfiguration config = null);
        
        event EventHandler<LoggerEventArgs> LoggerCreated;
    }
}
