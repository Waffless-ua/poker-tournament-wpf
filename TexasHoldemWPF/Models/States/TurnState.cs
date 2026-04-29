using TexasHoldemWPF.Enums;
using TexasHoldemWPF.ViewModels;

namespace TexasHoldemWPF.Models.States
{
    public class TurnState : IGameState
    {
        private const string STATE_MESSAGE = "Turn: Fourth card on the board";
        private const int COMMUNITY_CARDS_TO_DEAL = 1;

        public bool IsBettingComplete => false;

        public void Enter(GameViewModel context)
        {
            context.GameMessage = STATE_MESSAGE;
            context.DealCommunityCards(COMMUNITY_CARDS_TO_DEAL);
            context.CurrentBet = 0;
            context.ClearPlayerActions();
        }

        public string GetStateName() => "Turn";

        public GameState GetGameStateEnum() => GameState.Turn;

        public void HandleAction(GameViewModel context)
        {
            if (context.IsBettingRoundComplete())
            {
                context.TransitionToState(new RiverState());
            }
        }
    }
}