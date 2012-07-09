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
        private List<TargetBinding> Bindings { get; set; }
        private object _bindingsLock = new object();

        public LoggingConfiguration()
        {
            this.Bindings = new List<TargetBinding>();
        }

        public void AddTarget(LogLevel level, Target target)
        {
            this.AddTarget(level, level, target);
        }

        public void AddTarget(LogLevel min, LogLevel max, Target target)
        {
            lock (_bindingsLock)
                this.Bindings.Add(new TargetBinding(min, max, target));
        }

        internal LoggingConfiguration Clone()
        {
            throw new NotImplementedException();
        }

        internal IEnumerable<Target> GetTargets(LogLevel level)
        {
            var results = new List<Target>();
            foreach (var binding in this.Bindings.Where(v => v.SupportsLevel(level)))
                results.Add(binding.Target);

            return results;
        }

        public void ClearTargets()
        {
            lock (_bindingsLock)
                this.Bindings.Clear();
        }
    }
}
