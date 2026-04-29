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
using TexasHoldemWPF.ViewModels;
using TexasHoldemWPF.Views;
using TexasHoldemWPF.Resources;
namespace TexasHoldemWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Resources.NavigationService NavigationService { get; }

        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            NavigationService = new Resources.NavigationService(MainFrame);
            NavigateToMenu();
        }

        public void NavigateToMenu()
        {
            var menuViewModel = new MenuViewModel(NavigationService);
            NavigationService.NavigateTo<MenuView>(menuViewModel);
        }
    }
}
    