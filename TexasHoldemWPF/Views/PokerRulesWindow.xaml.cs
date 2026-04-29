using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace TexasHoldemWPF.Views
{
    /// <summary>
    /// Interaction logic for PokerRulesWindow.xaml
    /// </summary>
    public partial class PokerRulesWindow : Window
    {
        public ObservableCollection<string> BettingRules { get; }
        private bool isClosing = false;
        public PokerRulesWindow()
        {
            InitializeComponent();
            this.Left = 20;
            this.Top = 20;
            BettingRules = new ObservableCollection<string>
        {
            "• Small Blind/Big Blind: Forced bets that start the action",
            "• Check: Pass the action to next player without betting",
            "• Call: Match the current bet",
            "• Raise: Increase the current bet",
            "• Fold: Discard your hand and forfeit the pot",
            "• All-in: Bet all remaining chips"
        };
            DataContext = this;
            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.Background = Brushes.White;
            this.Topmost = true;
        }
        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            if (!isClosing)
            {
                isClosing = true;
                this.Close();
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isClosing)
            {
                isClosing = true;
                this.Close();
            }
        }
    }
}
