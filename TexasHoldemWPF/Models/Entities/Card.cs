using System;
using System.ComponentModel;
using TexasHoldemWPF.Enums;

namespace TexasHoldemWPF.Models.Entities
{
    public class Card : INotifyPropertyChanged
    {
        #region Property
        private Suit suit;
        public Suit Suit
        {
            get { return suit; }
            set { if (suit != value) { suit = value; OnPropertyChanged(nameof(Suit)); } }
        }
        public Rank rank;
        public Rank Rank
        {
            get { return rank; }
            set { if (rank != value) { rank = value; OnPropertyChanged(nameof(Rank)); } }
        }
        public string imagePath;
        public string ImagePath
        {
            get { return imagePath; }
            set { if (imagePath != value) { imagePath = value; OnPropertyChanged(nameof(ImagePath)); } }
        }
        private bool isInBestHand = false;
        public bool IsInBestHand
        {
            get => isInBestHand;
            set
            {
                isInBestHand = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
            ImagePath = $"pack://application:,,,/Resources/Cards/{GetRankShortName(Rank)}{GetSuitShortName(Suit)}.png";
        }

        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Card otherCard)
                return Suit == otherCard.Suit && Rank == otherCard.Rank;
            return false;
        }

        public override int GetHashCode()
        {
            return (int)Suit * 100 + (int)Rank;
        }

        public static bool operator ==(Card left, Card right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(Card left, Card right)
        {
            return !(left == right);
        }

        private string GetRankShortName(Rank rank)
        {
            switch (rank)
            {
                case Rank.Two: return "2";
                case Rank.Three: return "3";
                case Rank.Four: return "4";
                case Rank.Five: return "5";
                case Rank.Six: return "6";
                case Rank.Seven: return "7";
                case Rank.Eight: return "8";
                case Rank.Nine: return "9";
                case Rank.Ten: return "10";
                case Rank.Jack: return "J";
                case Rank.Queen: return "Q";
                case Rank.King: return "K";
                case Rank.Ace: return "A";
                default: throw new ArgumentOutOfRangeException(nameof(rank), rank, null);
            }
        }

        private string GetSuitShortName(Suit suit)
        {
            switch (suit)
            {
                case Suit.Hearts: return "H";
                case Suit.Diamonds: return "D";
                case Suit.Clubs: return "C";
                case Suit.Spades: return "S";
                default: throw new ArgumentOutOfRangeException(nameof(suit), suit, null);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}