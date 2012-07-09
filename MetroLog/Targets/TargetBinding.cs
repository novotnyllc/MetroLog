using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Targets
{
    internal class TargetBinding
    {
        internal LogLevel MinLevel { get; private set; }
        internal LogLevel MaxLevel { get; private set; }
        internal Target Target { get; private set; }

        internal TargetBinding(LogLevel min, LogLevel max, Target target)
        {
            this.MinLevel = min;
            this.MaxLevel = max;
            this.Target = target;
        }

        internal bool SupportsLevel(LogLevel level)
        {
            return (int)level >= (int)MinLevel && (int)level <= (int)MaxLevel;
        }
    }
}
