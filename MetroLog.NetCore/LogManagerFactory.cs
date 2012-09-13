using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Internal;
using MetroLog.Targets;
using Windows.UI.Xaml;

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

            var instance = factory.CreateNew(new LoggingEnvironment(), config);

            SetDefaultLogManager(instance);

            // initialize the suspend manager...
            SuspendManager.Initialize(instance);
        }

        protected override LoggingConfiguration CreateDefaultSettings()
        {
            var def = base.CreateDefaultSettings();
            def.AddTarget(LogLevel.Error, LogLevel.Fatal, new FileSnapshotTarget());

            return def;
        }
    }
}
