namespace TexasHoldemWPF.Models.Entities
{
    public class PokerHand
    {
        public string Name { get; }
        public string Description { get; }
        public string ImageName { get; }

        public PokerHand(string name, string description, string imageName)
        {
            Name = name;
            Description = description;
            ImageName = imageName;
        }

        public string ImagePath => $"pack://application:,,,/Resources/CardCombinations/{ImageName}";
    }
}