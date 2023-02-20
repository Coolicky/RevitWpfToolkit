using System;
using System.Globalization;
using System.Windows.Data;

namespace Coolicky.Revit.Toolkit.Wpf.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class InverseNullToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}