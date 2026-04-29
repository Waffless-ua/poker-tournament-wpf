using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TexasHoldemWPF.Controls
{
    public partial class ActionControl : UserControl
    {
        public static readonly DependencyProperty ActionTextProperty =
            DependencyProperty.Register("ActionText", typeof(string), typeof(ActionControl), new PropertyMetadata(""));

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(ActionControl),
            new PropertyMetadata(new SolidColorBrush(Colors.Green)));

        public string ActionText
        {
            get => (string)GetValue(ActionTextProperty);
            set => SetValue(ActionTextProperty, value);
        }

        public Brush BackgroundColor
        {
            get => (Brush)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public ActionControl()
        {
            InitializeComponent();
        }
    }
}