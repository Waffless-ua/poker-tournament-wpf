using System;
using System.Collections.Generic;
using System.Linq;
using TexasHoldemWPF.Enums;
using TexasHoldemWPF.Models.Entities;

namespace TexasHoldemWPF.Models.Utilities
{
    public class HandEvaluator
    {
        public struct HandEvaluationResult
        {
            public HandRank Rank;
            public List<Card> BestHand;
            public List<Card> Kickers;

            public HandEvaluationResult(HandRank rank, List<Card> bestHand, List<Card> kickers)
            {
                Rank = rank;
                BestHand = bestHand;
                Kickers = kickers;
            }
        }

        public static HandEvaluationResult EvaluateHand(List<Card> communityCards, List<Card> playerCards)
        {
            foreach (var card in communityCards.Concat(playerCards))
                card.IsInBestHand = false;
            var allCards = communityCards.Concat(playerCards).OrderByDescending(c => c.Rank).ToList();
            var result = GetBestHand(allCards);
            foreach (var card in result.BestHand)
                card.IsInBestHand = true;
            return result;
        }

        private static HandEvaluationResult GetBestHand(List<Card> allCards)
        {
            List<Card> combination;

            combination = GetRoyalFlush(allCards);
            if (combination != null)
                return new HandEvaluationResult(HandRank.RoyalFlush, combination, GetKickers(allCards, combination, 2));

            combination = GetStraightFlush(allCards);
            if (combination != null)
                return new HandEvaluationResult(HandRank.StraightFlush, combination, GetKickers(allCards, combination, 2));

            combination = GetFourOfAKind(allCards);
            if (combination != null)
                return new HandEvaluationResult(HandRank.FourOfAKind, combination, GetKickers(allCards, combination, 3));

            combination = GetFullHouse(allCards);
            if (combination != null)
                return new HandEvaluationResult(HandRank.FullHouse, combination, GetKickers(allCards, combination, 2));

            combination = GetFlush(allCards);
            if (combination != null)
                return new HandEvaluationResult(HandRank.Flush, combination, GetKickers(allCards, combination, 2));

            combination = GetStraight(allCards);
            if (combination != null)
                return new HandEvaluationResult(HandRank.Straight, combination, GetKickers(allCards, combination, 2));

            combination = GetThreeOfAKind(allCards);
            if (combination != null)
                return new HandEvaluationResult(HandRank.ThreeOfAKind, combination, GetKickers(allCards, combination, 4));

            combination = GetTwoPair(allCards);
            if (combination != null)
                return new HandEvaluationResult(HandRank.TwoPair, combination, GetKickers(allCards, combination, 3));

            combination = GetOnePair(allCards);
            if (combination != null)
                return new HandEvaluationResult(HandRank.OnePair, combination, GetKickers(allCards, combination, 5));

            combination = GetHighCards(allCards, 1);
            return new HandEvaluationResult(HandRank.HighCard, combination, GetKickers(allCards, combination, 6));
        }

        private static List<Card> GetKickers(List<Card> allCards, List<Card> combination, int kickersNeeded)
        {
            return allCards
                .Except(combination)
                .OrderByDescending(c => c.Rank)
                .Take(kickersNeeded)
                .ToList();
        }

        private static List<Card> GetRoyalFlush(List<Card> cards)
        {
            var straightFlush = GetStraightFlush(cards);
            if (straightFlush == null) return null;
            var royalRanks = new List<Rank> { Rank.Ace, Rank.King, Rank.Queen, Rank.Jack, Rank.Ten };
            if (straightFlush.All(c => royalRanks.Contains(c.Rank)))
                return straightFlush;
            return null;
        }

        private static List<Card> GetStraightFlush(List<Card> cards)
        {
            var flushCards = cards.GroupBy(c => c.Suit)
                                 .Where(g => g.Count() >= 5)
                                 .SelectMany(g => g)
                                 .OrderByDescending(c => c.Rank)
                                 .ToList();

            if (flushCards.Count < 5) return null;

            var straightFlush = GetStraight(flushCards);
            return straightFlush;
        }

        private static List<Card> GetFourOfAKind(List<Card> cards)
        {
            var fourOfAKind = GetNOfAKind(cards, 4);
            if (fourOfAKind == null) return null;

            return fourOfAKind;
        }

        private static List<Card> GetFullHouse(List<Card> cards)
        {
            var threeOfAKind = GetNOfAKind(cards, 3);
            if (threeOfAKind == null) return null;

            var remainingCards = cards.Except(threeOfAKind).ToList();
            var pair = GetNOfAKind(remainingCards, 2);
            if (pair == null) return null;

            return threeOfAKind.Concat(pair).ToList();
        }

        private static List<Card> GetFlush(List<Card> cards)
        {
            var flushGroup = cards.GroupBy(c => c.Suit)
                                  .FirstOrDefault(g => g.Count() >= 5);

            if (flushGroup == null) return null;

            return flushGroup.OrderByDescending(c => c.Rank)
                           .Take(5)
                           .ToList();
        }

        private static List<Card> GetStraight(List<Card> cards)
        {
            var distinctRanks = cards.Select(c => c.Rank)
                                    .Distinct()
                                    .OrderByDescending(r => r)
                                    .ToList();

            if (distinctRanks.Contains(Rank.Ace) &&
                distinctRanks.Contains(Rank.Five) &&
                distinctRanks.Contains(Rank.Four) &&
                distinctRanks.Contains(Rank.Three) &&
                distinctRanks.Contains(Rank.Two))
            {
                return cards.Where(c => c.Rank == Rank.Ace ||
                                      c.Rank == Rank.Five ||
                                      c.Rank == Rank.Four ||
                                      c.Rank == Rank.Three ||
                                      c.Rank == Rank.Two)
                           .OrderByDescending(c => c.Rank == Rank.Ace ? (int)Rank.Five + 1 : (int)c.Rank)
                           .Take(5)
                           .ToList();
            }

            for (int i = 0; i <= distinctRanks.Count - 5; i++)
            {
                if (distinctRanks[i] - distinctRanks[i + 4] == 4)
                {
                    return cards.Where(c => distinctRanks.GetRange(i, 5).Contains(c.Rank))
                               .GroupBy(c => c.Rank)
                               .Select(g => g.First())
                               .OrderByDescending(c => c.Rank)
                               .Take(5)
                               .ToList();
                }
            }

            return null;
        }

        private static List<Card> GetThreeOfAKind(List<Card> cards)
        {
            var threeOfAKind = GetNOfAKind(cards, 3);
            if (threeOfAKind == null) return null;

            return threeOfAKind;
        }

        private static List<Card> GetTwoPair(List<Card> cards)
        {
            var firstPair = GetNOfAKind(cards, 2);
            if (firstPair == null) return null;

            var remainingCards = cards.Except(firstPair).ToList();
            var secondPair = GetNOfAKind(remainingCards, 2);
            if (secondPair == null) return null;

            return firstPair.Concat(secondPair).ToList();
        }

        private static List<Card> GetOnePair(List<Card> cards)
        {
            var pair = GetNOfAKind(cards, 2);
            if (pair == null) return null;

            return pair;
        }

        private static List<Card> GetHighCards(List<Card> cards, int count)
        {
            return cards.OrderByDescending(c => c.Rank)
                       .Take(count)
                       .ToList();
        }

        private static List<Card> GetNOfAKind(List<Card> cards, int count)
        {
            var group = cards.GroupBy(c => c.Rank)
                           .OrderByDescending(g => g.Key)
                           .FirstOrDefault(g => g.Count() >= count);

            if (group == null) return null;

            return group.OrderByDescending(c => c.Rank)
                       .Take(count)
                       .ToList();
        }
    }
}