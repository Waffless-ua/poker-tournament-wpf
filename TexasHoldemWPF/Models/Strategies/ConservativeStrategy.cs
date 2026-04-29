using TexasHoldemWPF.Enums;

namespace TexasHoldemWPF.Models.Strategies
{
    public class ConservativeStrategy : BaseBotStrategy
    {
        private const double MIN_STRENGTH_TO_RAISE = 0.7;
        private const double RAISE_MULTIPLIER = 3;
        private const double RAISE_PROBABILITY = 0.4;
        private const double MIN_STRENGTH_TO_CALL = 0.5;
        private const double BLUFF_PROBABILITY = 0.05;
        private const double MIN_STRENGTH_FOR_BLUFF = 0.3;

        public override string StrategyName => "Conservative";

        public override ActionType Decide(DecisionContext context)
        {
            if (context.HandStrength > MIN_STRENGTH_TO_RAISE && context.Balance > context.CurrentBet * RAISE_MULTIPLIER)
            {
                if (_rng.NextDouble() < RAISE_PROBABILITY)
                    return ActionType.Raise;
                return ActionType.Call;
            }

            if (context.HandStrength > MIN_STRENGTH_TO_CALL)
                return ActionType.Call;

            if (_rng.NextDouble() < BLUFF_PROBABILITY && context.HandStrength > MIN_STRENGTH_FOR_BLUFF)
                return ActionType.Raise;

            return ActionType.Fold;
        }
    }
}