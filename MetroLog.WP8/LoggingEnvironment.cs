using System;


using MetroLog.Internal;

namespace MetroLog
{
    /// <summary>
    /// Holds the logging environment for .NET apps.
    /// </summary>
    public class LoggingEnvironment : LoggingEnvironmentBase
    {
        public LoggingEnvironment()
            : base(Environment.Version.ToString())
        {
            this.MachineName = "WP8Device";
        }
    }
}
