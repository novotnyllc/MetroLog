using MetroLog.Targets;

namespace MetroLog.Operators;

public interface ILogCompressor : ILogOperator
{
    Task<MemoryStream> GetCompressedLogs();
}