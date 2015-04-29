namespace MetroLog.Config.Converters
{
    internal class IntegerTypeConverter : TypeConverterBase
    {
        protected override object ConvertString(object source)
        {
            return int.Parse(source.ToString());
        }
    }
}