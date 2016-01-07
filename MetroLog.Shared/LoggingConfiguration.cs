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
        private readonly List<TargetBinding> _bindings;
        private readonly object _bindingsLock = new object();

        private bool _frozen;

        public LoggingConfiguration()
        {
            IsEnabled = true; // default to true to enable logging
            _bindings = new List<TargetBinding>();
        }

        public void AddTarget(LogLevel level, Target target)
        {
            AddTarget(level, level, target);
        }

        public void AddTarget(LogLevel min, LogLevel max, Target target)
        {
            if (_frozen)
                throw new InvalidOperationException("Cannot modify config after initialization");

            lock (_bindingsLock)
                _bindings.Add(new TargetBinding(min, max, target));
        }

        internal IEnumerable<Target> GetTargets()
        {
            lock (_bindingsLock)
            {
                var results = new List<Target>();
                foreach (var binding in _bindings)
                    results.Add(binding.Target);

                return results;
            }
        }

        internal IEnumerable<Target> GetTargets(LogLevel level)
        {
            lock(_bindingsLock)
                return _bindings.Where(v => v.SupportsLevel(level)).Select(binding => binding.Target).ToList();
        }

        internal void Freeze()
        {
            _frozen = true;
        }
    }
}
