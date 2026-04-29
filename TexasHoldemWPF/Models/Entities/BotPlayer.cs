using System;
using System.Collections.Generic;
using TexasHoldemWPF.Enums;
using TexasHoldemWPF.Models.Strategies;
using static TexasHoldemWPF.Models.Utilities.HandEvaluator;
using static TexasHoldemWPF.Models.Utilities.HandEvaluator;

namespace TexasHoldemWPF.Models.Entities
{
    public class BotPlayer : Player
    {
        private readonly Random _rng = new Random();
        private IBotStrategy _strategy;

        private const double STRENGTH_BASE_PAIR_HIGH = 0.8;
        private const double STRENGTH_INCREMENT_JACK_PLUS = 0.015;
        private const double STRENGTH_BASE_PAIR_MID = 0.6;
        private const double STRENGTH_INCREMENT_SEVEN_PLUS = 0.05;
        private const double STRENGTH_LOW_PAIR = 0.5;

        private const double STRENGTH_AK_SUITED = 0.85;
        private const double STRENGTH_KQ_SUITED = 0.75;
        private const double STRENGTH_QJ_SUITED = 0.65;

        private const double STRENGTH_SUITED_BONUS = 0.7;
        private const double STRENGTH_SUITED_MID = 0.6;
        private const double STRENGTH_SUITED_LOW = 0.5;

        private const double STRENGTH_CONNECTED_HIGH = 0.55;
        private const double STRENGTH_CONNECTED_MID = 0.5;
        private const double STRENGTH_WEAK = 0.3;

        private const int JACK_RANK = 11;
        private const int SEVEN_RANK = 7;
        private const int TEN_RANK = 10;

        private const double OUTS_HIGH = 12;
        private const double OUTS_MEDIUM = 8;
        private const double OUTS_LOW = 4;
        private const double IMPROVEMENT_HIGH = 0.35;
        private const double IMPROVEMENT_MEDIUM = 0.25;
        private const double IMPROVEMENT_LOW = 0.15;
        private const double IMPROVEMENT_MIN = 0.05;

        private const int MAX_OUTS = 15;

        public BotPlayer(string name, int initialBalance) : base(name, initialBalance)
        {
            _strategy = new ConservativeStrategy();
        }

        public BotPlayer(string name, int initialBalance, IBotStrategy strategy) : base(name, initialBalance)
        {
            _strategy = strategy;
        }

        public void SetStrategy(IBotStrategy strategy)
        {
            _strategy = strategy;
        }

        public string GetStrategyName()
        {
            return _strategy?.StrategyName ?? "Unknown";
        }

        public ActionType MakeDecision(int currentBet, int potSize, List<Card> communityCards)
        {
            double handStrength;
            if (communityCards.Count == 0)
                handStrength = PreflopStrength(Hand[0], Hand[1]);
            else
            {
                var eval = EvaluateHand(communityCards, Hand);
                handStrength = CalculateHandPotential(eval, communityCards.Count);
            }

            double potOdds = currentBet == 0 ? 0 : (double)currentBet / (potSize + currentBet);

            var context = new DecisionContext(handStrength, potOdds, currentBet, potSize, Balance, communityCards.Count);

            return _strategy.Decide(context);
        }

        private double PreflopStrength(Card c1, Card c2)
        {
            bool isPair = c1.Rank == c2.Rank;
            bool isSuited = c1.Suit == c2.Suit;
            int highCard = Math.Max((int)c1.Rank, (int)c2.Rank);
            int lowCard = Math.Min((int)c1.Rank, (int)c2.Rank);
            int cardGap = Math.Abs((int)c1.Rank - (int)c2.Rank);

            if (isPair)
            {
                if (highCard >= JACK_RANK)
                    return STRENGTH_BASE_PAIR_HIGH + (highCard - JACK_RANK) * STRENGTH_INCREMENT_JACK_PLUS;
                if (highCard >= SEVEN_RANK)
                    return STRENGTH_BASE_PAIR_MID + (highCard - SEVEN_RANK) * STRENGTH_INCREMENT_SEVEN_PLUS;
                return STRENGTH_LOW_PAIR;
            }

            if (highCard >= (int)Rank.Ace && lowCard >= TEN_RANK) return STRENGTH_AK_SUITED;
            if (highCard >= (int)Rank.King && lowCard >= (int)Rank.Queen) return STRENGTH_KQ_SUITED;
            if (highCard >= (int)Rank.Queen && lowCard >= (int)Rank.Jack) return STRENGTH_QJ_SUITED;

            if (isSuited)
            {
                if (highCard >= (int)Rank.King && cardGap <= 2) return STRENGTH_SUITED_BONUS;
                if (highCard >= (int)Rank.Jack && cardGap <= 3) return STRENGTH_SUITED_MID;
                if (cardGap <= 4) return STRENGTH_SUITED_LOW;
            }

            if (cardGap <= 2 && highCard >= TEN_RANK) return STRENGTH_CONNECTED_HIGH;
            if (cardGap <= 3 && highCard >= (int)Rank.Queen) return STRENGTH_CONNECTED_MID;

            return STRENGTH_WEAK;
        }

        private double CalculateHandPotential(HandEvaluationResult evaluation, int communityCardsCount)
        {
            double baseStrength = (double)evaluation.Rank / 10;
            int outs = CalculateOuts(communityCardsCount);
            double improvementChance = 0;

            if (communityCardsCount < 5)
            {
                if (outs > OUTS_HIGH) improvementChance = IMPROVEMENT_HIGH;
                else if (outs > OUTS_MEDIUM) improvementChance = IMPROVEMENT_MEDIUM;
                else if (outs > OUTS_LOW) improvementChance = IMPROVEMENT_LOW;
                else improvementChance = IMPROVEMENT_MIN;
            }

            return Math.Min(1.0, baseStrength + improvementChance);
        }

        private int CalculateOuts(int communityCardsCount)
        {
            if (communityCardsCount < 3) return 0;
            return _rng.Next(0, MAX_OUTS);
        }

        public override string GetStrategyInfo()
        {
            return $"Bot ({_strategy?.StrategyName ?? "Unknown"})";
        }
    }
}