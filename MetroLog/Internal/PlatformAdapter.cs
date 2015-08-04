using System;

namespace MetroLog.Internal
{
    // Enables types within PclContrib to use platform-specific features in a platform-agnostic way
    public static class PlatformAdapter
    {
        private static IAdapterResolver _resolver = new ProbingAdapterResolver();

        public static T Resolve<T>(bool throwIfNotFound = true, params object[] args)
        {
            Type type = typeof(T);
            T value = (T)_resolver.Resolve(type, args);

            if (value == null && throwIfNotFound)
            {
                throw new PlatformNotSupportedException(string.Format(Strings.AdapterNotSupported, type.FullName));
            }

            return value;
        }


        // Unit testing helper
        internal static void SetResolver(IAdapterResolver resolver)
        {
            _resolver = resolver;
        }
    }

}
