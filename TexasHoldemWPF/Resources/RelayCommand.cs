using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace TexasHoldemWPF.Resources
{
    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public RelayCommand(Action execute = null, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => canExecute?.Invoke() ?? true;
        public void Execute(object parameter) => execute?.Invoke();
    }
    public class RelayCommand<T> : ICommand
    {
        protected Action<T> execute;
        protected Func<T, bool> canExecute;



        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }



        public RelayCommand(Action<T> execute = null, Func<T, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }



        public bool CanExecute(T parameter) => canExecute?.Invoke(parameter) ?? true;
        public void Execute(T parameter) => execute?.Invoke(parameter);
        bool ICommand.CanExecute(object parameter) => parameter is T validParameter && CanExecute(validParameter);
        void ICommand.Execute(object parameter) { if (parameter is T validParameter) Execute(validParameter); }
    }
}
