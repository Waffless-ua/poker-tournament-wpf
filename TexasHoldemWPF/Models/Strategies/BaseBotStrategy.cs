using System;
using TexasHoldemWPF.Enums;

namespace TexasHoldemWPF.Models.Strategies
{
    public abstract class BaseBotStrategy : IBotStrategy
    {
        protected readonly Random _rng = new Random();

        public abstract ActionType Decide(DecisionContext context);
        public abstract string StrategyName { get; }
    }
}