using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace RS485.UI.Helpers.Converters
{
    /// <summary>
    /// Class NullToVisibilityConverter. This class cannot be inherited.
    /// </summary>
    [ValueConversion(typeof(object), typeof(Visibility))]
    public sealed class NullToVisibilityConverter : IValueConverter
    {
        private const string inverseParameter = "inverse";
        private const string hideParameter = "hide";

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
            string[] param = null;
            var parameterString = parameter as string;
            if (parameterString != null)
                param = parameterString.Split(' ', ',');

            bool isNull;

            var valueString = value as string;
            if (valueString != null)
                isNull = string.IsNullOrWhiteSpace(valueString);
            else
                isNull = value == null;

            if (param != null && param.Any(x => x.ToLowerInvariant() == inverseParameter))
                isNull = !isNull;

            if (param != null && param.Any(x => x.ToLowerInvariant() == hideParameter))
                return isNull ? Visibility.Hidden : Visibility.Visible;

            return isNull ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

    }
}
