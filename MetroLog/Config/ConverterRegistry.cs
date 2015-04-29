using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

using MetroLog.Config.Converters;

namespace MetroLog.Config
{
    public static class ConverterRegistry
    {
        private static readonly Dictionary<Type, object> TypeConverters = new Dictionary<Type, object>();

        static ConverterRegistry()
        {
            AddConverter(typeof(object), typeof(StringTypeConverter));
            AddConverter(typeof(string), typeof(StringTypeConverter));
            AddConverter(typeof(int), typeof(IntegerTypeConverter));
            AddConverter(typeof(double), typeof(DoubleTypeConverter));
            AddConverter(typeof(bool), typeof(BooleanTypeConverter));
            AddConverter(typeof(Uri), typeof(UriTypeConverter));
        }

        public static void AddConverter(Type destinationType, object converter)
        {
            if (destinationType != null && converter != null)
            {
                lock (TypeConverters)
                {
                    TypeConverters[destinationType] = converter;
                }
            }
        }

        public static void AddConverter(Type destinationType, Type converterType)
        {
            AddConverter(destinationType, CreateConverterInstance(converterType));
        }

        public static object ConvertTo(Type destinationType, object value)
        {
            var typeConverter = GetConverterForType(destinationType);
            if (typeConverter != null)
            {
                // Found appropriate converter
                return typeConverter.Convert(value);
            }
            else
            {
                if (destinationType.GetTypeInfo().IsEnum)
                {
                    return Enum.Parse(destinationType, value.ToString(), true);
                }
                else
                {
                    // We essentially make a guess that to convert from a string
                    // to an arbitrary type T there will be a static method defined on type T called Parse
                    // that will take an argument of type string. i.e. T.Parse(string)->T we call this
                    // method to convert the string to the type required by the property.
                    MethodInfo parseMethod = destinationType.GetRuntimeMethod("Parse", new[] { typeof(string) });
                    if (parseMethod != null)
                    {
                        // Call the Parse method
                        return parseMethod.Invoke(value, new object[] { });
                    }
                }
            }

            return null;
        }

        public static ITypeConverter GetConverterForType(Type destinationType)
        {
            lock (TypeConverters)
            {
                // Lookup in the static registry
                if (TypeConverters.ContainsKey(destinationType))
                {
                    var converter = TypeConverters[destinationType] as ITypeConverter;
                    if (converter == null)
                    {
                        // Lookup using attributes
                        converter = CreateConverterInstance(destinationType) as ITypeConverter;
                        if (converter != null)
                        {
                            // Store in registry
                            TypeConverters[destinationType] = converter;
                        }
                    }

                    return converter;
                }

                return null;
            }
        }

        private static object CreateConverterInstance(Type converterType)
        {
            if (converterType == null)
            {
                throw new ArgumentNullException("converterType", "CreateConverterInstance cannot create instance, converterType is null");
            }

            // Check type is a converter
            if (typeof(ITypeConverter).GetTypeInfo().IsAssignableFrom(converterType.GetTypeInfo()) /*|| typeof(IConvertTo).IsAssignableFrom(converterType)*/)
            {
                try
                {
                    // Create the type converter
                    return Activator.CreateInstance(converterType);
                }
                catch (Exception ex)
                {
                    ////LogLog.Error(DeclaringType, "Cannot CreateConverterInstance of type [" + converterType.FullName + "], Exception in call to Activator.CreateInstance", ex);
                }
            }
            else
            {
                ////LogLog.Error(DeclaringType, "Cannot CreateConverterInstance of type [" + converterType.FullName + "], type does not implement ITypeConverter or IConvertTo");
            }
            return null;
        }
    }
}