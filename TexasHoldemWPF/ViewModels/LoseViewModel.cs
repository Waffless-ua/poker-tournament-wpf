using System;
using System.Threading.Tasks;
using System.Windows;
using TexasHoldemWPF.Resources;
using TexasHoldemWPF.Views;
namespace TexasHoldemWPF.ViewModels
{
    public class LoseViewModel : BaseViewModel
    {
        private readonly NavigationService _navigationService;

        public string Message { get; } = "You lost!";
        public string SubMessage { get; } = "Just remember, that the house always wins";

        public LoseViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            Task.Delay(5000).ContinueWith(_ =>
                Application.Current.Dispatcher.Invoke(() =>
                    _navigationService.NavigateTo<MenuView>(new MenuViewModel(_navigationService))));
        }
    }
}
