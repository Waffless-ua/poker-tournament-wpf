namespace TexasHoldemWPF.Models.Entities
{
    public class Tournament
    {
        public string City { get; }
        public int Prize { get; }
        public int BuyIn { get; }
        public string ImagePath { get; }
        public double SmallBlind { get; }
        public double BigBlind { get; }
        public double MinBet { get; }

        public Tournament(string city, int prize, int buyIn, string imagePath)
        {
            City = city;
            Prize = prize;
            BuyIn = buyIn;
            ImagePath = imagePath;
            SmallBlind = buyIn * 0.01;
            BigBlind = buyIn * 0.02;
            MinBet = buyIn * 0.05;
        }
    }
}