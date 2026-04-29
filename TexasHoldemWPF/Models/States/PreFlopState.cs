using TexasHoldemWPF.Enums;
using TexasHoldemWPF.ViewModels;

namespace TexasHoldemWPF.Models.States
{
    public class PreFlopState : IGameState
    {
        private const string STATE_MESSAGE = "Pre-flop: Place your bet!";

        public bool IsBettingComplete => false;

        public void Enter(GameViewModel context)
        {
            context.GameMessage = STATE_MESSAGE;
            context.ShowBlindsInfo = true;
            context.PostBlinds();
        }

        public string GetStateName() => "Pre-Flop";

        public GameState GetGameStateEnum() => GameState.PreFlop;

        public void HandleAction(GameViewModel context)
        {
            if (context.IsBettingRoundComplete())
            {
                context.TransitionToState(new FlopState());
            }
        }
    }
}