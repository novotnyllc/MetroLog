using System;

namespace MetroLog.Config.Converters
{
    internal class DoubleTypeConverter : TypeConverterBase
    {
        protected override object ConvertString(object source)
        {
            return double.Parse(source.ToString());
        }
    }
}