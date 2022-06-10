using System.Diagnostics;

namespace MetroLog.Targets;

[DebuggerDisplay("Name = {Target.GetType().Name}, Min = {MinLevel}, Max = {MaxLevel}")]
internal class TargetBinding
{
    internal TargetBinding(LogLevel min, LogLevel max, Target target)
    {
        MinLevel = min;
        MaxLevel = max;
        Target = target;
    }

    internal Target Target { get; }

    private LogLevel MinLevel { get; }

    private LogLevel MaxLevel { get; }

    internal bool SupportsLevel(LogLevel level)
    {
        return (int)level >= (int)MinLevel && (int)level <= (int)MaxLevel;
    }
}