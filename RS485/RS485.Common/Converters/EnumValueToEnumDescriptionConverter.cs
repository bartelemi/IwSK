using System;
using System.ComponentModel;
using System.Windows.Data;

namespace RS485.Common.Converters
{
    [ValueConversion(typeof(Enum), typeof(string))]
    public class EnumValueToEnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var valueInfo = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[]) valueInfo.GetCustomAttributes(
                typeof (DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}