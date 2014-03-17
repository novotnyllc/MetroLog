using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Internal
{
    // Enables types within PclContrib to use platform-specific features in a platform-agnostic way
    internal static class PlatformAdapter
    {
        private static IAdapterResolver _resolver = new ProbingAdapterResolver();

        public static T Resolve<T>(bool throwIfNotFound = true, params object[] args)
        {
            T value = (T)_resolver.Resolve(typeof(T), args);

            if (value == null && throwIfNotFound)
                throw new PlatformNotSupportedException(Strings.AdapterNotSupported);

            return value;
        }


        // Unit testing helper
        internal static void SetResolver(IAdapterResolver resolver)
        {
            _resolver = resolver;
        }
    }

}
