using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TexasHoldemWPF.Resources.Converters
{
    public class ActionToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string action)
            {
                if (action.Contains("Small Blind") || action.Contains("Big Blind"))
                    return new SolidColorBrush(Color.FromRgb(200, 200, 200)); 
                if (action.Contains("Call") || action.Contains("Check"))
                    return new SolidColorBrush(Colors.Green);
                if (action.Contains("Raise") || action.Contains("Bet") || action.Contains("All-in"))
                    return new SolidColorBrush(Colors.Red);
            }
            return new SolidColorBrush(Colors.Green);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}