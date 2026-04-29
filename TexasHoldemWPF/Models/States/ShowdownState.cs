using TexasHoldemWPF.Enums;
using TexasHoldemWPF.ViewModels;

namespace TexasHoldemWPF.Models.States
{
    public class ShowdownState : IGameState
    {
        private const string STATE_MESSAGE = "Showdown! Revealing hands...";

        public bool IsBettingComplete => true;

        public void Enter(GameViewModel context)
        {
            context.GameMessage = STATE_MESSAGE;
            context.ShowAllCards = true;
            context.EndGame();
        }

        public string GetStateName() => "Showdown";

        public GameState GetGameStateEnum() => GameState.River;

        public void HandleAction(GameViewModel context)
        {
            context.EndGame();
        }
    }
}