using System;
using System.Collections.Generic;
using TexasHoldemWPF.Enums;
using TexasHoldemWPF.Models.Entities;
using TexasHoldemWPF.Models.Strategies;

namespace TexasHoldemWPF.Models.Factory
{
    public class HumanPlayerFactory : IPlayerFactory
    {
        public Player CreatePlayer(int buyIn)
        {
            return new Player("You", buyIn);
        }
    }
}