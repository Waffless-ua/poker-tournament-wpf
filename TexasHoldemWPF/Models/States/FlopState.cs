using TexasHoldemWPF.Enums;
using TexasHoldemWPF.ViewModels;

namespace TexasHoldemWPF.Models.States
{
    public class FlopState : IGameState
    {
        private const string STATE_MESSAGE = "Flop: Three cards on the board";
        private const int COMMUNITY_CARDS_TO_DEAL = 3;

        public bool IsBettingComplete => false;

        public void Enter(GameViewModel context)
        {
            context.GameMessage = STATE_MESSAGE;
            context.ShowBlindsInfo = false;
            context.DealCommunityCards(COMMUNITY_CARDS_TO_DEAL);
            context.CurrentBet = 0;
            context.ClearPlayerActions();
        }

        public string GetStateName() => "Flop";

        public GameState GetGameStateEnum() => GameState.Flop;

        public void HandleAction(GameViewModel context)
        {
            if (context.IsBettingRoundComplete())
            {
                context.TransitionToState(new TurnState());
            }
        }
    }
}