
using System.Collections.Generic;
using System.Reflection;

namespace MetroLog
{
    public interface IAssemblyService
    {
        Assembly GetExecutingAssembly();

        IEnumerable<Assembly> GetAssemblies();
    }
}
