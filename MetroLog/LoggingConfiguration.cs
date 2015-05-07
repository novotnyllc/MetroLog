using System;
using System.Collections.Generic;
using System.Linq;

using MetroLog.Internal;
using MetroLog.Targets;

namespace MetroLog
{
    public class LoggingConfiguration
    {
        private readonly List<TargetBinding> bindings;
        private readonly object bindingsLock = new object();

        private bool isFrozen;

        public LoggingConfiguration()
        {
            this.IsEnabled = true; // default to true to enable logging
            this.CrashRecorder = new CrashRecorder(10); // TODO: add configuration to xmlConfigurator
            this.CrashRecorder.IsEnabled = true;
            this.bindings = new List<TargetBinding>();
        }

        public ICrashRecorder CrashRecorder { get; private set; }

        public bool IsEnabled { get; set; }

        public void AddTarget(LogLevel level, Target target)
        {
            this.AddTarget(level, level, target);
        }

        public void AddTarget(LogLevel min, LogLevel max, Target target)
        {
            if (this.isFrozen)
            {
                throw new InvalidOperationException("Cannot modify config after initialization");
            }

            lock (this.bindingsLock)
            {
                this.bindings.Add(new TargetBinding(min, max, target));
            }
        }

        internal IEnumerable<Target> GetTargets()
        {
            lock (this.bindingsLock)
            {
                return this.bindings.Select(binding => binding.Target).ToList();
            }
        }

        internal IEnumerable<Target> GetTargets(LogLevel level)
        {
            lock (this.bindingsLock)
            {
                return this.bindings.Where(v => v.SupportsLevel(level)).Select(binding => binding.Target).ToList();
            }
        }

        internal void Freeze()
        {
            this.isFrozen = true;
        }
    }
}