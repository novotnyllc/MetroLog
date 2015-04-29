using System;

namespace MetroLog.Config.Converters
{
    internal class UriTypeConverter : TypeConverterBase
    {
        protected override object ConvertString(object source)
        {
            return new Uri(source.ToString());
        }
    }
}