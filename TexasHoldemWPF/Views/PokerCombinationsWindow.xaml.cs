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
using System.Windows.Shapes;
using TexasHoldemWPF.Resources;
using TexasHoldemWPF.ViewModels;

namespace TexasHoldemWPF.Views
{
    
    /// <summary>
    /// Interaction logic for PokerRulesWindow.xaml
    /// </summary>
    public partial class PokerCombinationsWindow : Window
    {
        private bool isClosing = false;
        public PokerCombinationsWindow()
        {
            InitializeComponent();
            this.DataContext = new PokerCombinationsViewModel();
            this.Left = 20;
            this.Top = 20;
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
