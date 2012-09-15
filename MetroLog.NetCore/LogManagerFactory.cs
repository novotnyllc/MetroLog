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
    /// <summary>
    /// Defines a class that allows the log manager to be configured for Windows Store apps.
    /// </summary>
    public class LogManagerFactory : LogManagerFactoryBase
    {
        protected LogManagerFactory()
        {
            
        }

        /// <summary>
        /// Initializes the log manager.
        /// </summary>
        /// <param name="config">The (optional) logging configuration to make as the default.</param>
        public static void Initialize(LoggingConfiguration config = null)
        {
            var factory = new LogManagerFactory();

            var instance = factory.CreateNew(new LoggingEnvironment(), config);

            SetDefaultLogManager(instance);

            // initialize the suspend manager...
            LazyFlushManager.Initialize(instance);
        }

        /// <summary>
        /// Gets the default settings for Windows Store apps.
        /// </summary>
        /// <returns></returns>
        protected override LoggingConfiguration CreateDefaultSettings()
        {
            var def = base.CreateDefaultSettings();
            def.AddTarget(LogLevel.Error, LogLevel.Fatal, new FileSnapshotTarget());

            return def;
        }

        /// <summary>
        /// Configures a global exception handler.
        /// </summary>
        /// <remarks>This method will bind a global handler that will write the exception information to FATAL.
        /// It will then throw another exception which will stop the application (as per MS's Windows Store
        /// UX guidelines).</remarks>
        public static void ConfigureGlobalHandler()
        {
            Application.Current.UnhandledException += App_UnhandledException;
        }

        private static async void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // unbind we're going to re-enter and don't want to loop...
            Application.Current.UnhandledException -= App_UnhandledException;

            // say we've handled this one. this allows our FATAL write to complete.
            e.Handled = true;

            // go...
            var log = (ILoggerAsync)DefaultLogManager.GetLogger<LogManagerFactory>();
            await log.FatalAsync("The application crashed: " + e.Message, e.Exception);

            // if we're aborting, fake a suspend to flush the targets...
            await LazyFlushManager.FlushAllAsync();

            // abort the app here...
            Application.Current.Exit();
        }
    }
}
