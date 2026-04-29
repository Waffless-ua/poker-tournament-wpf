using TexasHoldemWPF.Enums;
using TexasHoldemWPF.ViewModels;

namespace TexasHoldemWPF.Models.States
{
    public class RiverState : IGameState
    {
        private const string STATE_MESSAGE = "River: Final card on the board";
        private const int COMMUNITY_CARDS_TO_DEAL = 1;

        public bool IsBettingComplete => false;

        public void Enter(GameViewModel context)
        {
            context.GameMessage = STATE_MESSAGE;
            context.DealCommunityCards(COMMUNITY_CARDS_TO_DEAL);
            context.CurrentBet = 0;
            context.ClearPlayerActions();
        }

        public string GetStateName() => "River";

        public GameState GetGameStateEnum() => GameState.River;

        public void HandleAction(GameViewModel context)
        {
            if (context.IsBettingRoundComplete())
            {
                context.EndGame();
            }
        }
    }
}