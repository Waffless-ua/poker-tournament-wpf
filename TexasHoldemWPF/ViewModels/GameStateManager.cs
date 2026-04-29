using System.Collections.Generic;
using System.Linq;
using TexasHoldemWPF.Enums;
using TexasHoldemWPF.Models.States;

namespace TexasHoldemWPF.ViewModels
{
    public class GameStateManager
    {
        private readonly GameViewModel _context;
        private IGameState _currentGameState;
        private readonly Dictionary<GameState, IGameState> _states;

        public GameStateManager(GameViewModel context)
        {
            _context = context;
            _states = new Dictionary<GameState, IGameState>
            {
                { GameState.PreFlop, new PreFlopState() },
                { GameState.Flop, new FlopState() },
                { GameState.Turn, new TurnState() },
                { GameState.River, new RiverState() }
            };
            _currentGameState = _states[GameState.PreFlop];
        }

        public void TransitionToState(IGameState newState)
        {
            _currentGameState = newState;
            _context.CurrentGameState = newState.GetGameStateEnum();
            _currentGameState.Enter(_context);
        }

        public bool IsBettingRoundComplete()
        {
            var activePlayers = _context.GetActivePlayers();

            bool allCalled = activePlayers.All(p =>
                p.CurrentBet == _context.CurrentBet ||
                p.Balance == 0
            );
            bool onlyOneActive = activePlayers.Count <= 1;

            return allCalled || onlyOneActive;
        }

        public void AdvanceGameState()
        {
            _currentGameState.HandleAction(_context);
        }

        public IGameState GetCurrentState()
        {
            return _currentGameState;
        }

        public string GetCurrentStateName()
        {
            return _currentGameState?.GetStateName() ?? "Unknown";
        }

        public void ResetToPreFlop()
        {
            _currentGameState = _states[GameState.PreFlop];
            _context.CurrentGameState = GameState.PreFlop;
        }
    }
}