using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;
using Windows.UI.Xaml;

namespace MetroLog
{
    /// <summary>
    /// Handles application suspension.
    /// </summary>
    /// <remarks>
    /// This class maintains a list of the managers and tracks application suspension. When the app is suspended,
    /// any discovered targets tht implement ISuspendNotify are called.
    /// </remarks>
    public class LazyFlushManager
    {
        ILogManager Owner { get; set; }
        List<ILazyFlushable> Clients { get; set; }
        ThreadPoolTimer Timer { get; set; }
        object _lock = new object();

        static Dictionary<ILogManager, LazyFlushManager> Owners { get; set; }

        LazyFlushManager(ILogManager owner)
        {
            this.Owner = owner;
            this.Owner.LoggerCreated += Owner_LoggerCreated;
            
            // clients...
            this.Clients = new List<ILazyFlushable>();

            // timer...
            Timer = ThreadPoolTimer.CreatePeriodicTimer(async (args) =>
            {
                await this.LazyFlushAsync(new LogWriteContext());

            }, TimeSpan.FromMinutes(2));
        }

        void Owner_LoggerCreated(object sender, LoggerEventArgs e)
        {
            lock(_lock)
            {
                foreach (var target in ((ILoggerQuery)e.Logger).GetTargets())
                {
                    if (target is ILazyFlushable && !(this.Clients.Contains((ILazyFlushable)target)))
                        this.Clients.Add((ILazyFlushable)target);
                }
            }
        }

        static LazyFlushManager()
        {
            Owners = new Dictionary<ILogManager, LazyFlushManager>();

            // only if we have an application...
#pragma warning disable 436 
            if(LoggingEnvironment.XamlApplicationState == XamlApplicationState.Available)
                Application.Current.Suspending += Current_Suspending;
#pragma warning restore 436
        }

        static async void Current_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            await FlushAllAsync(new LogWriteContext());
        }

        public static async Task FlushAllAsync(LogWriteContext context)
        {
            var tasks = new List<Task>();
            foreach (var manager in Owners.Values)
                tasks.Add(manager.LazyFlushAsync(context));

            // wait...
            await Task.WhenAll(tasks);
        }

        async Task LazyFlushAsync(LogWriteContext context)
        {
            List<ILazyFlushable> toNotify = null;
            lock (_lock)
                toNotify = new List<ILazyFlushable>(this.Clients);

            // walk...
            if (toNotify.Any())
            {
                var tasks = toNotify.Select(client => client.LazyFlushAsync(context)).ToList();

                // wait...
                await Task.WhenAll(tasks);
            }
        }

        internal static void Initialize(ILogManager manager)
        {
            Owners[manager] = new LazyFlushManager(manager);
        }
    }
}
