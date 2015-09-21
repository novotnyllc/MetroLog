namespace MetroLog
{
    public interface ILogManagerCreator
    {
        ILogManager Create(LoggingConfiguration configuration);
    }
}