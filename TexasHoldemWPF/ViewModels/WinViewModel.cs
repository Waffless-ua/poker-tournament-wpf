using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using TexasHoldemWPF.Resources;
using TexasHoldemWPF.Views;
namespace TexasHoldemWPF.ViewModels
{
    public class WinViewModel : BaseViewModel
    {
        private static readonly byte[] _aesKey = Encoding.UTF8.GetBytes("k9H2pLm4Wq8ZxRt7"); 
        private static readonly byte[] _aesIV = Encoding.UTF8.GetBytes("J3mN7vBt1QsPfYl9");
        private readonly NavigationService _navigationService;
        private readonly int _prize;

        public string Message { get; } = "You won!";
        public string SubMessage { get; } = "But remember, that the house always wins";

        public WinViewModel(NavigationService navigationService, int prize)
        {
            _navigationService = navigationService;
            _prize = prize;
            var balanceFile = "playerBalance.json";
            var encrypted = File.ReadAllText(balanceFile);
            var decrypted = DecryptString(encrypted, _aesKey, _aesIV);
            var balance = JsonSerializer.Deserialize<double>(decrypted);
            balance += _prize;
            var updatedJson = JsonSerializer.Serialize(balance);
            var updatedEncrypted = EncryptString(updatedJson, _aesKey, _aesIV);
            File.WriteAllText(balanceFile, updatedEncrypted);


            Task.Delay(5000).ContinueWith(_ =>
                Application.Current.Dispatcher.Invoke(() =>
                    _navigationService.NavigateTo<MenuView>(new MenuViewModel(_navigationService))));
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
