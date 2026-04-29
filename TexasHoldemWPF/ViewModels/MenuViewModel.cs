using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using TexasHoldemWPF.Models.Entities;
using TexasHoldemWPF.Resources;
using TexasHoldemWPF.Views;
namespace TexasHoldemWPF.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private static readonly byte[] _aesKey = Encoding.UTF8.GetBytes("k9H2pLm4Wq8ZxRt7"); 
        private static readonly byte[] _aesIV = Encoding.UTF8.GetBytes("J3mN7vBt1QsPfYl9");
        private double _playerBalance;
        private const string BalanceFile = "playerBalance.json";
        private readonly NavigationService _navigationService;
        public double PlayerBalance
        {
            get => _playerBalance;
            set
            {
                _playerBalance = value;
                OnPropertyChanged();
                SaveBalance();
            }
        }

        public ObservableCollection<Tournament> Tournaments { get; } = new ObservableCollection<Tournament>
        {
            new Tournament("Paris", 30000, 10000, "pack://application:,,,/Resources/Images/Paris.jpg"),
            new Tournament("Rio de Janeiro", 250000, 75000, "pack://application:,,,/Resources/images/Rio.jpg"),
            new Tournament("Sydney", 1500000, 500000, "pack://application:,,,/Resources/images/Sydney.jpg"),
            new Tournament("Tokyo", 10000000, 3000000, "pack://application:,,,/Resources/images/Tokyo.jpg")
        };

        public ICommand AddMoneyCommand { get; }
        public ICommand StartTournamentCommand { get; }
        public ICommand ShowRulesCommand { get; }
        public MenuViewModel(NavigationService navigationService)
        {
            LoadBalance();
            _navigationService = navigationService;
            AddMoneyCommand = new RelayCommand(AddMoney);
            StartTournamentCommand = new RelayCommand<Tournament>(StartTournament);
            ShowRulesCommand = new RelayCommand(ShowRules);
        }

        private void AddMoney()
        {
            PlayerBalance += 10000;
        }

        private void StartTournament(Tournament tournament)
        {
            if (PlayerBalance >= tournament.BuyIn)
            {
                PlayerBalance -= tournament.BuyIn;
                var gameViewModel = new GameViewModel(tournament, _navigationService);
                _navigationService.NavigateTo(new GameView { DataContext = gameViewModel });
            }
            else
            {
                MessageBox.Show("Not enough money to join this tournament!", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void ShowRules()
        {
            var rulesWindow = new PokerRulesWindow();
            rulesWindow.Show();
        }
        private void SaveBalance()
        {
            var json = JsonSerializer.Serialize(PlayerBalance);
            var encrypted = EncryptString(json, _aesKey, _aesIV);
            File.WriteAllText(BalanceFile, encrypted);
        }

        private void LoadBalance()
        {
            if (File.Exists(BalanceFile))
            {
                var encrypted = File.ReadAllText(BalanceFile);
                var decrypted = DecryptString(encrypted, _aesKey, _aesIV);
                PlayerBalance = JsonSerializer.Deserialize<double>(decrypted);
            }
            else
            {
                PlayerBalance = 10000;
            }
        }
        private string EncryptString(string plainText, byte[] key, byte[] iv)
        {
            var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        private string DecryptString(string cipherText, byte[] key, byte[] iv)
        {
            var buffer = Convert.FromBase64String(cipherText);

            var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            var ms = new MemoryStream(buffer);
            var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }

    }

}