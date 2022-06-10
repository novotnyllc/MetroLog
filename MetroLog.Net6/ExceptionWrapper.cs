using System.Text.Json;

namespace MetroLog;

public class ExceptionWrapper
{
    public ExceptionWrapper()
    {
    }

    internal ExceptionWrapper(Exception? ex)
    {
        TypeName = ex.GetType().AssemblyQualifiedName;
        AsString = ex.ToString();
        Hresult = ex.HResult;
    }

    public string TypeName { get; }

    public string AsString { get; }

    public int Hresult { get; }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}