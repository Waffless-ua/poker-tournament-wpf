using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TexasHoldemWPF.Controls
{
    public partial class MenuControl : UserControl
    {
        public static readonly DependencyProperty CityProperty =
            DependencyProperty.Register("City", typeof(string), typeof(MenuControl), new PropertyMetadata(""));

        public static readonly DependencyProperty PrizeProperty =
            DependencyProperty.Register("Prize", typeof(int), typeof(MenuControl), new PropertyMetadata(0));

        public static readonly DependencyProperty BuyInProperty =
            DependencyProperty.Register("BuyIn", typeof(int), typeof(MenuControl), new PropertyMetadata(0));

        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register("ImagePath", typeof(string), typeof(MenuControl), new PropertyMetadata(""));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(MenuControl));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(MenuControl));

        public string City
        {
            get => (string)GetValue(CityProperty);
            set => SetValue(CityProperty, value);
        }

        public int Prize
        {
            get => (int)GetValue(PrizeProperty);
            set => SetValue(PrizeProperty, value);
        }

        public int BuyIn
        {
            get => (int)GetValue(BuyInProperty);
            set => SetValue(BuyInProperty, value);
        }

        public string ImagePath
        {
            get => (string)GetValue(ImagePathProperty);
            set => SetValue(ImagePathProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public MenuControl()
        {
            InitializeComponent();
        }
    }
}