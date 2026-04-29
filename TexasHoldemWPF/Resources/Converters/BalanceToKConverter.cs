using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TexasHoldemWPF.Resources.Converters
{
    public class BalanceToKConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine($"Converting value: {value} (type: {value?.GetType().Name})");
            if (value is int balance)
            {
                if(balance >= 1000000)
                {
                    double kValue = balance / 1000000.0;
                    return $"${kValue:0.#}M";
                }
                else if (balance >= 1000)
                {
                    double kValue = balance / 1000.0;
                    return $"${kValue:0.#}K";
                }
                return $"${balance}";
            }
            return "$0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
