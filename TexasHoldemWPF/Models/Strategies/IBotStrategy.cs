using TexasHoldemWPF.Enums;

namespace TexasHoldemWPF.Models.Strategies
{
    public interface IBotStrategy
    {
        ActionType Decide(DecisionContext context);
        string StrategyName { get; }
    }
}