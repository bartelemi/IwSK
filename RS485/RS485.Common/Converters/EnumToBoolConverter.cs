using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace RS485.Common.Converters
{
    [ValueConversion(typeof(Enum), typeof(bool))]
    public class EnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            string enumValue = value.ToString();
            var targetValues = parameter.ToString().Split('|');

            return targetValues.Any(x => x.Equals(enumValue, StringComparison.InvariantCulture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
