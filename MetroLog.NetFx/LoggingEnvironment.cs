using MetroLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    /// <summary>
    /// Holds the logging environment for .NET apps.
    /// </summary>
    public class LoggingEnvironment : LoggingEnvironmentBase
    {
        static LoggingEnvironment()
        {
            var assm = typeof(AssemblyFileVersionAttribute).GetTypeInfo().Assembly;

            var ver = assm.GetCustomAttribute<AssemblyFileVersionAttribute>();

            Version = ver?.Version;

            var prod = assm.GetCustomAttribute<AssemblyProductAttribute>();
            Product = prod?.Product;



        }

        static readonly string Version;
        static readonly string Product;

#if !DOTNET
        public string MachineName { get; private set; }
#endif

        public LoggingEnvironment()
            : base($"{Product} - {Version}")
        {
#if !DOTNET
            MachineName = Environment.MachineName;
#endif
        }
    }
}
