using MetroLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    /// <summary>
    /// Holds the logging environment for .NET apps.
    /// </summary>
    public class LoggingEnvironment : LoggingEnvironmentBase
    {
        public string MachineName { get; private set; }

        public LoggingEnvironment()
            : base(Environment.Version.ToString())
        {
            this.MachineName = "WP8Device";
        }
    }
}
