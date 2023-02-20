using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Coolicky.Revit.Toolkit.Wpf.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class InverseBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool boolValue)) throw new ArgumentException("Argument is not a boolean");
            return boolValue ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility visibilityValue)) throw new ArgumentException("Argument is not a visibility");
            return visibilityValue == Visibility.Visible;
        }
    }
}