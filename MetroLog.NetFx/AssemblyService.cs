using System;
using System.Collections.Generic;
using System.Reflection;

namespace MetroLog
{
    internal sealed class AssemblyService : IAssemblyService
    {
        public Assembly GetExecutingAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}
