namespace MetroLog;

public interface ILoggingEnvironment
{
    Guid SessionId { get; }

    string ToJson();
}