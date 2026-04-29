using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TexasHoldemWPF.Resources.Converters
{
    public class ActionToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string action && !string.IsNullOrEmpty(action) && !action.Contains("Fold"))
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}