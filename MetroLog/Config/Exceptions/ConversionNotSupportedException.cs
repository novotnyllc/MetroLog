using System;

namespace MetroLog.Config.Exceptions
{
    public class ConversionNotSupportedException : Exception
    {
        public ConversionNotSupportedException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">A message to include with the exception.</param>
        /// <remarks>
        /// <para>
        /// Initializes a new instance of the <see cref="ConversionNotSupportedException" /> class
        /// with the specified message.
        /// </para>
        /// </remarks>
        public ConversionNotSupportedException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">A message to include with the exception.</param>
        /// <param name="innerException">A nested exception to include.</param>
        /// <remarks>
        /// <para>
        /// Initializes a new instance of the <see cref="ConversionNotSupportedException" /> class
        /// with the specified message and inner exception.
        /// </para>
        /// </remarks>
        public ConversionNotSupportedException(String message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ConversionNotSupportedException" /> class.
        /// </summary>
        /// <param name="destinationType">The conversion destination type.</param>
        /// <param name="sourceValue">The value to convert.</param>
        /// <param name="innerException">A nested exception to include.</param>
        /// <returns>An instance of the <see cref="ConversionNotSupportedException" />.</returns>
        /// <remarks>
        /// <para>
        /// Creates a new instance of the <see cref="ConversionNotSupportedException" /> class.
        /// </para>
        /// </remarks>
        public static ConversionNotSupportedException Create(Type destinationType, object sourceValue, Exception innerException = null)
        {
            if (sourceValue == null)
            {
                return new ConversionNotSupportedException("Cannot convert value [null] to type [" + destinationType + "]", innerException);
            }
            else
            {
                return new ConversionNotSupportedException("Cannot convert from type [" + sourceValue.GetType() + "] value [" + sourceValue + "] to type [" + destinationType + "]", innerException);
            }
        }
    }
}