namespace MetroLog;

public interface ILazyFlushable
{
    Task LazyFlushAsync(LogWriteContext context);
}