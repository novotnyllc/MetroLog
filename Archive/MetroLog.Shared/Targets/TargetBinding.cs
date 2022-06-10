using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Targets
{
    [DebuggerDisplay("Name = {Target.GetType().Name}, Min = {MinLevel}, Max = {MaxLevel}")]
    class TargetBinding
    {
        LogLevel MinLevel { get; }
        LogLevel MaxLevel { get; }
        internal Target Target { get; }

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
