using MetroLog.Targets;

namespace MetroLog.Operators;

public interface ILogLister : ILogOperator
{
    Task<List<string>> GetLogList();
}
