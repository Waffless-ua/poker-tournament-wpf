using System;
using System.Collections.Generic;
using TexasHoldemWPF.Models.Entities;
using TexasHoldemWPF.Models.Strategies;

namespace TexasHoldemWPF.Models.Factory
{
    public enum PlayerType
    {
        Human,
        AggressiveBot,
        ConservativeBot,
        TightBot,
        LooseBot
    }

    public static class PlayerFactory
    {
        private static readonly Random _rng = new Random();
        private static List<string> _botNames = new List<string> { "Alex", "John", "Mike", "Steve", "David", "Paul", "Chris", "Tom" };

        public static Player CreatePlayer(PlayerType type, int buyIn)
        {
            switch (type)
            {
                case PlayerType.Human:
                    return new Player("You", buyIn);

                case PlayerType.AggressiveBot:
                    return new BotPlayer(GetUniqueBotName(), buyIn, new AggressiveStrategy());

                case PlayerType.ConservativeBot:
                    return new BotPlayer(GetUniqueBotName(), buyIn, new ConservativeStrategy());

                case PlayerType.TightBot:
                    return new BotPlayer(GetUniqueBotName(), buyIn, new TightStrategy());

                case PlayerType.LooseBot:
                    return new BotPlayer(GetUniqueBotName(), buyIn, new LooseStrategy());

                default:
                    throw new ArgumentException("Unknown player type");
            }
        }

        public static Player CreateRandomBot(int buyIn)
        {
            PlayerType[] botTypes = new PlayerType[]
            {
                PlayerType.AggressiveBot,
                PlayerType.ConservativeBot,
                PlayerType.TightBot,
                PlayerType.LooseBot
            };

            PlayerType randomType = botTypes[_rng.Next(botTypes.Length)];
            return CreatePlayer(randomType, buyIn);
        }

        private static string GetUniqueBotName()
        {
            if (_botNames.Count == 0)
            {
                return "Bot_" + Guid.NewGuid().ToString().Substring(0, 4);
            }

            int index = _rng.Next(_botNames.Count);
            string name = _botNames[index];
            _botNames.RemoveAt(index);
            return name;
        }
    }
}