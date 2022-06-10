using MetroLog.Targets;

namespace MetroLog.Operators;

public interface ILogOperatorRetriever
{
    bool TryGetOperator<TInterface>(out TInterface? @operator) where TInterface : ILogOperator;
}

public class LogOperatorRetriever : ILogOperatorRetriever
{
    private static readonly List<ILogManager> Managers = new();

    internal static void AddManager(ILogManager manager)
    {
        Managers.Add(manager);
    }

    public static readonly ILogOperatorRetriever Instance = new LogOperatorRetriever();

    public bool TryGetOperator<TInterface>(out TInterface? @operator) where TInterface : ILogOperator
    {
        foreach (var logManager in Managers)
        {
            if (logManager.TryGetOperator(out @operator))
            {
                return true;
            }
        }

        @operator = default;
        return false;
    }
}
