
namespace MetroLog.Layouts
{
    /// <summary>
    /// Defines the base class for string-based layouts.
    /// </summary>
    public abstract class Layout
    {
        protected Layout()
        {
        }

        /// <summary>
        /// Returns the formatted log event.
        /// </summary>
        /// <param name="context">The logging context.</param>
        /// <param name="logEventInfo">The log event for format.</param>
        /// <returns>The foramtted string.</returns>
        public abstract string GetFormattedString(LogWriteContext context, LogEventInfo logEventInfo);
    }
}
