using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TexasHoldemWPF.Controls
{
    /// <summary>
    /// Interaction logic for BalanceControl.xaml
    /// </summary>
    public partial class BalanceControl : UserControl
    {
        public static readonly DependencyProperty BalanceProperty =
            DependencyProperty.Register("Balance", typeof(double), typeof(BalanceControl));

        public static readonly DependencyProperty AddMoneyCommandProperty =
            DependencyProperty.Register("AddMoneyCommand", typeof(ICommand), typeof(BalanceControl));

        public double Balance
        {
            get => (double)GetValue(BalanceProperty);
            set => SetValue(BalanceProperty, value);
        }

        public ICommand AddMoneyCommand
        {
            get => (ICommand)GetValue(AddMoneyCommandProperty);
            set => SetValue(AddMoneyCommandProperty, value);
        }

        public BalanceControl()
        {
            InitializeComponent();
        }
    }
}
