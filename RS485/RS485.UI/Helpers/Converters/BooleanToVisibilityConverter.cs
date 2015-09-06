using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RS485.UI.Helpers.Converters
{
    /// <summary>
    /// Class BooleanToVisibilityConverter. This class cannot be inherited.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the true value.
        /// </summary>
        /// <value>The true value.</value>
        public Visibility TrueValue { get; set; }
        /// <summary>
        /// Gets or sets the false value.
        /// </summary>
        /// <value>The false value.</value>
        public Visibility FalseValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanToVisibilityConverter"/> class.
        /// </summary>
        public BooleanToVisibilityConverter()
        {
            // set defaults
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        #region IValueConverter members
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
            if (value is bool)
                return (bool)value ? TrueValue : FalseValue;

            return null;
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
            if (Equals(value, TrueValue))
                return true;
            if (Equals(value, FalseValue))
                return false;
            return null;
        }
        #endregion
    }
}
