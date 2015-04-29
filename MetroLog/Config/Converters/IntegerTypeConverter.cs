namespace MetroLog.Config.Converters
{
    internal class BooleanTypeConverter : TypeConverterBase
    {
        protected override object ConvertString(object source)
        {
            return int.Parse(source.ToString());
        }
    }
}