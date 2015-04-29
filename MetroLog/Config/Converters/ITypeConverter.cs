namespace MetroLog.Config.Converters
{
    public interface ITypeConverter
    {
        /// <summary>
        ///     Convert the source object to the type supported by this object
        /// </summary>
        /// <param name="source">the object to convert</param>
        /// <returns>the converted object</returns>
        /// <remarks>
        ///     Converts the <paramref name="source" /> to the type supported
        ///     by this converter.
        /// </remarks>
        object Convert(object source);
    }
}