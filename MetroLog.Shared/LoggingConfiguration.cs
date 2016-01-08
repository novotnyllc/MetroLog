using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Targets;

namespace MetroLog
{
    public class LoggingConfiguration
    {
        public bool IsEnabled { get; set; }
        readonly List<TargetBinding> bindings;
        readonly object bindingsLock = new object();

        bool frozen;

        public LoggingConfiguration()
        {
            IsEnabled = true; // default to true to enable logging
            bindings = new List<TargetBinding>();
        }

        public void AddTarget(LogLevel level, Target target)
        {
            AddTarget(level, level, target);
        }

        public void AddTarget(LogLevel min, LogLevel max, Target target)
        {
            if (frozen)
                throw new InvalidOperationException("Cannot modify config after initialization");

            lock (bindingsLock)
                bindings.Add(new TargetBinding(min, max, target));
        }

        internal IEnumerable<Target> GetTargets()
        {
            lock (bindingsLock)
            {
                var results = new List<Target>();
                foreach (var binding in bindings)
                    results.Add(binding.Target);

                return results;
            }
        }

        internal IEnumerable<Target> GetTargets(LogLevel level)
        {
            lock(bindingsLock)
                return bindings.Where(v => v.SupportsLevel(level)).Select(binding => binding.Target).ToList();
        }

        internal void Freeze()
        {
            frozen = true;
        }
    }
}
