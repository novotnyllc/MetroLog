using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MetroLog
{
    /// <summary>
    /// Handles application suspension.
    /// </summary>
    internal class SuspendManager
    {
        private ILogManager Manager { get; set; }
        private List<ISuspendNotify> Clients { get; set; }
        private object _lock = new object();

        private static Dictionary<ILogManager, SuspendManager> Managers { get; set; }

        private SuspendManager(ILogManager manager)
        {
            this.Manager = manager;
            this.Clients = new List<ISuspendNotify>();
            manager.LoggerCreated += manager_LoggerCreated;
        }

        void manager_LoggerCreated(object sender, ILoggerEventArgs e)
        {
            var query = (ILoggerQuery)e.Logger;
            foreach (var target in query.GetTargets())
            {
                if (target is ISuspendNotify)
                {
                    lock (_lock)
                        this.Clients.Add((ISuspendNotify)target);
                }
            }
        }

        static SuspendManager()
        {
            Managers = new Dictionary<ILogManager, SuspendManager>();
            Application.Current.Suspending += Current_Suspending;
        }

        static void Current_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            foreach (var manager in Managers.Values)
                manager.Suspending();
        }

        private void Suspending()
        {
            List<ISuspendNotify> toNotify = null;
            lock(_lock)
                toNotify = new List<ISuspendNotify>(this.Clients);

            // walk...
            if (toNotify.Any())
            {
                var context = this.Manager.GetWriteContext();
                foreach (var client in toNotify)
                    client.Suspending(context);
            }
        }

        internal static void Initialize(ILogManager manager)
        {
            Managers[manager] = new SuspendManager(manager);
        }
    }
}
