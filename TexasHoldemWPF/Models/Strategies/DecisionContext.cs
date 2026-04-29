namespace TexasHoldemWPF.Models.Strategies
{
    public class DecisionContext
    {
        public double HandStrength { get; set; }
        public double PotOdds { get; set; }
        public int CurrentBet { get; set; }
        public int PotSize { get; set; }
        public int Balance { get; set; }
        public int CommunityCardsCount { get; set; }

        public DecisionContext(double handStrength, double potOdds, int currentBet,
                               int potSize, int balance, int communityCardsCount)
        {
            HandStrength = handStrength;
            PotOdds = potOdds;
            CurrentBet = currentBet;
            PotSize = potSize;
            Balance = balance;
            CommunityCardsCount = communityCardsCount;
        }
    }
}