using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TexasHoldemWPF.Models.Entities
{
    public class Player : INotifyPropertyChanged
    {
        #region Property
        private string name;
        public string Name
        {
            get { return name; }
            set { if (name != value) { name = value; OnPropertyChanged(nameof(Name)); } }
        }

        private int balance;
        public int Balance
        {
            get { return balance; }
            set { if (balance != value) { balance = value; OnPropertyChanged(nameof(Balance)); } }
        }

        private List<Card> hand = new List<Card>();
        public List<Card> Hand
        {
            get { return hand; }
            set { if (hand != value) { hand = value; OnPropertyChanged(nameof(Hand)); } }
        }

        private int currentBet;
        public int CurrentBet
        {
            get { return currentBet; }
            set { if (currentBet != value) { currentBet = value; OnPropertyChanged(nameof(CurrentBet)); } }
        }

        private bool isFolded;
        public bool IsFolded
        {
            get { return isFolded; }
            set { if (isFolded != value) { isFolded = value; OnPropertyChanged(nameof(IsFolded)); } }
        }

        private string lastAction = "";
        public string LastAction
        {
            get { return lastAction; }
            set { if (lastAction != value) { lastAction = value; OnPropertyChanged(nameof(LastAction)); } }
        }
        #endregion

        public Player(string name, int balance)
        {
            Name = name;
            Balance = balance;
            Hand = new List<Card>();
            CurrentBet = 0;
            IsFolded = false;
            LastAction = "";
        }

        public virtual string GetStrategyInfo()
        {
            return "Human Player";
        }

        public virtual void ResetForNewRound()
        {
            CurrentBet = 0;
            if (Balance <= 0)
                IsFolded = true;
            else
                IsFolded = false;
            LastAction = "";
        }

        public bool CanCall(int currentBetAmount)
        {
            return Balance >= (currentBetAmount - CurrentBet);
        }

        public int GetCallAmount(int currentBetAmount)
        {
            return Math.Min(currentBetAmount - CurrentBet, Balance);
        }

        public bool CanRaise(int currentBetAmount, int minRaise)
        {
            return Balance >= minRaise && Balance >= currentBetAmount + minRaise;
        }

        public bool IsAllIn()
        {
            return Balance == 0;
        }

        public void PlaceBet(int amount)
        {
            if (amount > Balance)
                amount = Balance;
            Balance -= amount;
            CurrentBet += amount;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}