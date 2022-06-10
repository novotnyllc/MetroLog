using MetroLog.Targets;

namespace MetroLog;

public interface ILoggerQuery
{
    IEnumerable<Target> GetTargets();
}