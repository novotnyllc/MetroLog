using System;
using System.Collections.Generic;
using System.Linq;

using MetroLog.Targets;

namespace MetroLog
{
    public class LoggingConfiguration
    {
        public bool IsEnabled { get; set; }

        private readonly List<TargetBinding> _bindings;
        private readonly object _bindingsLock = new object();

        private bool _frozen;

        public LoggingConfiguration()
        {
            this.IsEnabled = true; // default to true to enable logging
            this._bindings = new List<TargetBinding>();
        }

        public void AddTarget(LogLevel level, Target target)
        {
            this.AddTarget(level, level, target);
        }

        public void AddTarget(LogLevel min, LogLevel max, Target target)
        {
            if (this._frozen)
            {
                throw new InvalidOperationException("Cannot modify config after initialization");
            }

            lock (this._bindingsLock) this._bindings.Add(new TargetBinding(min, max, target));
        }

        internal IEnumerable<Target> GetTargets()
        {
            lock (this._bindingsLock)
            {
                var results = new List<Target>();
                foreach (var binding in this._bindings)
                {
                    results.Add(binding.Target);
                }

                return results;
            }
        }

        internal IEnumerable<Target> GetTargets(LogLevel level)
        {
            lock (this._bindingsLock) return this._bindings.Where(v => v.SupportsLevel(level)).Select(binding => binding.Target).ToList();
        }

        internal void Freeze()
        {
            this._frozen = true;
        }
    }
}