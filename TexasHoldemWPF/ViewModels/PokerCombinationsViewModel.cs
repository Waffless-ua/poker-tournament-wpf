using System.Collections.ObjectModel;
using TexasHoldemWPF.Models.Entities;
namespace TexasHoldemWPF.ViewModels
{
    public class PokerCombinationsViewModel: BaseViewModel
    {
        public ObservableCollection<PokerHand> Hands { get; } = new ObservableCollection<PokerHand>
        {
            new PokerHand(
                "Royal Flush",
                "A, K, Q, J, 10 all of the same suit. The highest possible hand.",
                "RoyalFlush.png"),

            new PokerHand(
                "Straight Flush",
                "Five sequential cards of the same suit.",
                "StraightFlush.png"),

            new PokerHand(
                "Four of a Kind",
                "Four cards of the same rank.",
                "FourOfAKind.png"),

            new PokerHand(
                "Full House",
                "Three of a kind plus a pair.",
                "FullHouse.png"),

            new PokerHand(
                "Flush",
                "Five cards of the same suit, not in sequence.",
                "Flush.png"),

            new PokerHand(
                "Straight",
                "Five sequential cards of mixed suits.",
                "Straight.png"),

            new PokerHand(
                "Three of a Kind",
                "Three cards of the same rank.",
                "ThreeOfAKind.png"),

            new PokerHand(
                "Two Pair",
                "Two different pairs.",
                "TwoPair.png"),

            new PokerHand(
                "One Pair",
                "One pair of cards.",
                "OnePair.png"),

            new PokerHand(
                "High Card",
                "No combinations, highest card plays.",
                "HighCard.png")
        };
    }

}