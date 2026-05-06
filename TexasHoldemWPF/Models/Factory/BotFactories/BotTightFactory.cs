using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldemWPF.Enums;
using TexasHoldemWPF.Models.Entities;
using TexasHoldemWPF.Models.Strategies;
using TexasHoldemWPF.Models.Utilities;

namespace TexasHoldemWPF.Models.Factory.BotFactories
{
    public class BotTightFactory : IPlayerFactory
    {
        public Player CreatePlayer(int buyIn)
        {
            return new BotPlayer(BotNameGenerator.GetUniqueBotName(), buyIn, new TightStrategy());
        }
    }
}
