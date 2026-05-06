using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldemWPF.Enums;
using TexasHoldemWPF.Models.Entities;

namespace TexasHoldemWPF.Models.Factory
{
    public interface IPlayerFactory
    {
        Player CreatePlayer(int buyIn);
    }
}
