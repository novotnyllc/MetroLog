using System.IO;
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

        /// <summary>
        /// Returns a zip archive stream of the logs folder
        /// </summary>
        /// <returns>Null if no file logger is attached</returns>
        Task<Stream> GetCompressedLogs();
    }
}
