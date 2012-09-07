using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    public class LogManagerFactory : LogManagerFactoryBase
    {
        protected LogManagerFactory()
        {
            
        }

        public static void Initialize(LoggingConfiguration config = null)
        {
            var factory = new LogManagerFactory();

            var instance = factory.CreateNew(config);

            SetDefaultLogManager(instance);
        }

    }
}
