using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemWPF.Models.Utilities
{
    public static class BotNameGenerator
    {
        private static readonly Random _rng = new Random();
        private static List<string> _botNames = new List<string> { 
            "Alex", "John", "Mike", 
            "Steve", "David", "Paul", 
            "Chris", "Tom" 
        };

        public static string GetUniqueBotName()
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
