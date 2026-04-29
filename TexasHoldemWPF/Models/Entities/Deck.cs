using System;
using System.Collections.Generic;
using System.Linq;
using TexasHoldemWPF.Enums;

namespace TexasHoldemWPF.Models.Entities
{
    public class Deck
    {
        private List<Card> _cards;
        private Random _rng = new Random();

        public Deck()
        {
            _cards = new List<Card>();

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                    _cards.Add(new Card(suit, rank));
            Shuffle();
        }

        public void Shuffle()
        {
            _cards = _cards.OrderBy(_ => _rng.Next()).ToList();
        }

        public Card DrawCard()
        {
            if (_cards.Count == 0) return null;
            Card drawnCard = _cards[0];
            _cards.RemoveAt(0);
            return drawnCard;
        }

        public int CardsRemaining => _cards.Count;
    }
}