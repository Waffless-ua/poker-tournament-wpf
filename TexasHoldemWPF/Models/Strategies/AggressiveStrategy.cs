using TexasHoldemWPF.Enums;

namespace TexasHoldemWPF.Models.Strategies
{
    public class AggressiveStrategy : BaseBotStrategy
    {
        private const double MIN_STRENGTH_TO_RAISE = 0.35;
        private const double RAISE_MULTIPLIER = 2;
        private const double AGGRESSION_RAISE_PROBABILITY = 0.7;
        private const double MIN_STRENGTH_TO_CALL = 0.2;
        private const double BLUFF_PROBABILITY = 0.2;

        public override string StrategyName => "Aggressive";

        public override ActionType Decide(DecisionContext context)
        {
            if (context.HandStrength > MIN_STRENGTH_TO_RAISE && context.Balance > context.CurrentBet * RAISE_MULTIPLIER)
            {
                if (_rng.NextDouble() < AGGRESSION_RAISE_PROBABILITY)
                    return ActionType.Raise;
            }

            if (context.HandStrength > MIN_STRENGTH_TO_CALL)
                return ActionType.Call;

            if (_rng.NextDouble() < BLUFF_PROBABILITY && context.Balance > context.CurrentBet)
                return ActionType.Raise;

            return ActionType.Fold;
        }
    }
}