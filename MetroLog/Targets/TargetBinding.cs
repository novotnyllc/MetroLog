using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Targets
{
    [DebuggerDisplay("Name = {Target.GetType().Name}, Min = {MinLevel}, Max = {MaxLevel}")]
    internal class TargetBinding
    {
        private LogLevel MinLevel { get; set; }
        private LogLevel MaxLevel { get; set; }
        internal Target Target { get; private set; }

        internal TargetBinding(LogLevel min, LogLevel max, Target target)
        {
            MinLevel = min;
            MaxLevel = max;
            Target = target;
        }

        internal bool SupportsLevel(LogLevel level)
        {
            return (int)level >= (int)MinLevel && (int)level <= (int)MaxLevel;
        }
    }
}
