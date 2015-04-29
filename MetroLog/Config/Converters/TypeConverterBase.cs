using System;

using MetroLog.Config.Exceptions;

namespace MetroLog.Config.Converters
{
    internal abstract class TypeConverterBase : ITypeConverter
    {
        private bool CanConvertFrom(Type sourceType)
        {
            return (sourceType == typeof(string));
        }

        protected abstract object ConvertString(object source);

        public object Convert(object source)
        {
            //TODO CHeck of string null

            if (this.CanConvertFrom(source.GetType()))
            {
                return this.ConvertString(source);
            }

            throw new ConversionNotSupportedException();
        }
    }
}