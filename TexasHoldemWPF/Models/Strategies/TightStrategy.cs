using TexasHoldemWPF.Enums;

namespace TexasHoldemWPF.Models.Strategies
{
    public class TightStrategy : BaseBotStrategy
    {
        private const double BASE_REQUIRED_STRENGTH = 0.6;
        private const double POST_FLOP_REDUCTION = 0.05;
        private const double HIGH_STRENGTH_FOR_RAISE = 0.8;
        private const double RAISE_MULTIPLIER = 4;
        private const double POT_ODDS_BUFFER = 0.1;

        public override string StrategyName => "Tight";

        public override ActionType Decide(DecisionContext context)
        {
            double requiredStrength = BASE_REQUIRED_STRENGTH;

            if (context.CommunityCardsCount >= 3)
                requiredStrength = BASE_REQUIRED_STRENGTH - POST_FLOP_REDUCTION;

            if (context.HandStrength > requiredStrength &&
                context.HandStrength > context.PotOdds + POT_ODDS_BUFFER)
            {
                if (context.HandStrength > HIGH_STRENGTH_FOR_RAISE && context.Balance > context.CurrentBet * RAISE_MULTIPLIER)
                    return ActionType.Raise;
                return ActionType.Call;
            }

            return ActionType.Fold;
        }
    }
}