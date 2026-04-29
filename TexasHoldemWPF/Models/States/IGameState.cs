using TexasHoldemWPF.Enums;
using TexasHoldemWPF.ViewModels;

namespace TexasHoldemWPF.Models.States
{
    public interface IGameState
    {
        void Enter(GameViewModel context);
        void HandleAction(GameViewModel context);
        string GetStateName();
        bool IsBettingComplete { get; }
        GameState GetGameStateEnum();
    }
}