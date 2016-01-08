using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MetroLog
{
    public static class GlobalCrashHandler
    {
        public static void Configure()
        {
            Application.Current.UnhandledException += App_UnhandledException;
        }

        static async void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // unbind we're going to re-enter and don't want to loop...
            Application.Current.UnhandledException -= App_UnhandledException;

            // say we've handled this one. this allows our FATAL write to complete.
            e.Handled = true;

            // go...
            var log = (ILoggerAsync)LogManagerFactory.DefaultLogManager.GetLogger<Application>();
            await log.FatalAsync("The application crashed: " + e.Message, e);

            // if we're aborting, fake a suspend to flush the targets...
            await LazyFlushManager.FlushAllAsync(new LogWriteContext());

            // abort the app here...
            Application.Current.Exit();
        }
    }
}
