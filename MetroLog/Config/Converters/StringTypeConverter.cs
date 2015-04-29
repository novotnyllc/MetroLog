namespace MetroLog.Config.Converters
{
    internal class StringTypeConverter : TypeConverterBase
    {
        protected override object ConvertString(object source)
        {
            return source.ToString(); // No need to convert here
        }
    }
}