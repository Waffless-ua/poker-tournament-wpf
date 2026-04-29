using System;
using System.Globalization;
using System.Windows.Data;

namespace TexasHoldemWPF.Resources.Converters
{
    public class RaiseOptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int raiseAmount) || !(parameter is string optionsText))
                return value.ToString();

            string[] options = optionsText.Split('|');
            int[] amounts = value as int[] ?? Array.Empty<int>();

            int index = Array.IndexOf(amounts, raiseAmount);
            return index >= 0 && index < options.Length ? $"{options[index]} (${raiseAmount})" : raiseAmount.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}