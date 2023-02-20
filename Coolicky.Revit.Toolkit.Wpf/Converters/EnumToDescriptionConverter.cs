using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Coolicky.Revit.Toolkit.Wpf.Converters
{
    [ValueConversion(typeof(Enum), typeof(string))]
    public class EnumToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Enum enumValue)) return new ArgumentException("Argument is not an Enum");
            var attr = enumValue.GetType().GetField(enumValue.ToString())
                .GetCustomAttributes(typeof (DescriptionAttribute), false);

            return attr.Length > 0 ? ((DescriptionAttribute) attr[0]).Description : enumValue.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string stringValue)) throw new ArgumentException("Argument is not a string");
            return Enum.Parse(targetType, stringValue);
        }
    }
}