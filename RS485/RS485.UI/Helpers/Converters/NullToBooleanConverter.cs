using System;
using System.Globalization;
using System.Windows.Data;

namespace RS485.UI.Helpers.Converters
{
    /// <summary>
    /// Class NullToBooleanConverter. This class cannot be inherited.
    /// </summary>
    [ValueConversion(typeof(object), typeof(string))]
    public sealed class NullToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = parameter as string;
            bool isNull;

            var str = value as string;
            if (str != null)
                isNull = string.IsNullOrWhiteSpace(str);
            else
                isNull = value == null;

            if (param != null && param == "Inverse")
                isNull = !isNull;

            return isNull;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}