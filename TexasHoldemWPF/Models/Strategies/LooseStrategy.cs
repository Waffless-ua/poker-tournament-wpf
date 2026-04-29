using TexasHoldemWPF.Enums;

namespace TexasHoldemWPF.Models.Strategies
{
    public class LooseStrategy : BaseBotStrategy
    {
        private const double MIN_STRENGTH_TO_RAISE = 0.25;
        private const double RAISE_PROBABILITY = 0.5;
        private const double BLUFF_PROBABILITY = 0.15;
        private const double MIN_STRENGTH_TO_CALL = 0.1;
        private const double FOLD_THRESHOLD = 0.1;

        public override string StrategyName => "Loose";

        public override ActionType Decide(DecisionContext context)
        {
            if (context.HandStrength > MIN_STRENGTH_TO_RAISE)
            {
                if (_rng.NextDouble() < RAISE_PROBABILITY && context.Balance > context.CurrentBet)
                    return ActionType.Raise;
                return ActionType.Call;
            }

            if (_rng.NextDouble() < BLUFF_PROBABILITY && context.Balance > context.CurrentBet)
                return ActionType.Raise;

            if (context.HandStrength < FOLD_THRESHOLD)
                return ActionType.Fold;

            return ActionType.Call;
        }
    }
}