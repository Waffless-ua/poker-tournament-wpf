using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using TexasHoldemWPF.Enums;
using TexasHoldemWPF.Models.Entities;
using TexasHoldemWPF.Models.Factory;
using TexasHoldemWPF.Models.Factory.BotFactories;
using TexasHoldemWPF.Models.States;
using TexasHoldemWPF.Resources;
using TexasHoldemWPF.Views;

namespace TexasHoldemWPF.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        #region Constants
        public const int DEFAULT_BET_AMOUNT = 100;
        public const int BOT_ACTION_DELAY_MS = 800;
        public const int STATE_TRANSITION_DELAY_MS = 500;
        public const int SHOWDOWN_DELAY_MS = 3000;
        public const int EARLY_WIN_DELAY_MS = 1000;
        public const int BOT_START_INDEX = 1;
        public const int LAST_BOT_INDEX = 4;
        public const int FIRST_BOT_INDEX = 1;
        public const int BET_DISPLAY_DURATION_SECONDS = 2;
        #endregion

        #region Private Fields
        private int _currentBet;
        private int _potSize;
        private int _playerBalance;
        private string _gameMessage;
        private GameState _gameState;
        private List<Player> _players;
        private Deck _deck;
        private int _dealerPosition;
        private DispatcherTimer _betDisplayTimer;
        private string _currentBetDisplay;
        private bool _showBlindsInfo = true;
        private bool _canAct = true;
        private string _raiseOptionsText = "";
        private bool _isRaiseMenuVisible;
        private List<int> _raiseOptions = new List<int>();
        private bool _isWaitingForPlayerAction;
        private int _bigBlind;
        private int _buyIn;
        private int _prize;
        private bool _showAllCards;
        private string _callCheckButtonText = "Call";
        private bool _isEnded = false;

        private ICommand _betCommand;
        private ICommand _raiseCommand;
        private ICommand _callCommand;
        private ICommand _foldCommand;
        private ICommand _executeRaiseCommand;
        private ICommand _cancelCommand;
        private ICommand _returnToMenuCommand;
        private ICommand _showCombinationsCommand;
        #endregion

        #region Managers
        public GameStateManager StateManager { get; private set; }
        public GameBettingManager BettingManager { get; private set; }
        public GameBotManager BotManager { get; private set; }
        public GameWinnerManager WinnerManager { get; private set; }
        public GameRoundManager RoundManager { get; private set; }
        #endregion

        #region Collections
        public ObservableCollection<Card> PlayerCards { get; set; }
        public ObservableCollection<Card> Bot1Cards { get; set; }
        public ObservableCollection<Card> Bot2Cards { get; set; }
        public ObservableCollection<Card> Bot3Cards { get; set; }
        public ObservableCollection<Card> Bot4Cards { get; set; }
        public ObservableCollection<Card> CommunityCards { get; set; }
        #endregion

        #region Public Properties
        public int CurrentBet { get => _currentBet; set { _currentBet = value; OnPropertyChanged(); } }
        public int PotSize { get => _potSize; set { _potSize = value; OnPropertyChanged(); } }
        public int PlayerBalance { get => _playerBalance; set { _playerBalance = value; OnPropertyChanged(); } }
        public string GameMessage { get => _gameMessage; set { _gameMessage = value; OnPropertyChanged(); } }
        public string CurrentBetDisplay { get => _currentBetDisplay; set { _currentBetDisplay = value; OnPropertyChanged(); } }
        public bool ShowBlindsInfo { get => _showBlindsInfo; set { _showBlindsInfo = value; OnPropertyChanged(); } }
        public bool CanAct { get => _canAct; set { _canAct = value; OnPropertyChanged(); } }
        public bool IsRaiseMenuVisible { get => _isRaiseMenuVisible; set { _isRaiseMenuVisible = value; OnPropertyChanged(); } }
        public List<int> RaiseOptions { get => _raiseOptions; set { _raiseOptions = value; OnPropertyChanged(); } }
        public string RaiseOptionsText { get => _raiseOptionsText; set { _raiseOptionsText = value; OnPropertyChanged(); } }
        public bool IsWaitingForPlayerAction { get => _isWaitingForPlayerAction; set { _isWaitingForPlayerAction = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanAct)); } }
        public int DealerPosition { get => _dealerPosition; set { _dealerPosition = value; OnPropertyChanged(); OnPropertyChanged(nameof(DealerPositions)); OnPropertyChanged(nameof(SmallBlindPositions)); OnPropertyChanged(nameof(BigBlindPositions)); } }
        public bool ShowAllCards { get => _showAllCards; set { _showAllCards = value; OnPropertyChanged(); } }
        public string CallCheckButtonText { get => _callCheckButtonText; set { _callCheckButtonText = value; OnPropertyChanged(); } }
        public List<Player> Players { get => _players; set { _players = value; OnPropertyChanged(); } }
        public Deck CurrentDeck { get => _deck; set => _deck = value; }
        public int BigBlindAmount => _bigBlind;
        public int BuyInAmount => _buyIn;
        public int PrizeAmount => _prize;
        public bool IsEndedFlag => _isEnded;
        public GameState CurrentGameState { get => _gameState; set => _gameState = value; }
        #endregion

        #region Commands (Public Properties з get тільки)
        public ICommand BetCommand => _betCommand;
        public ICommand RaiseCommand => _raiseCommand;
        public ICommand CallCommand => _callCommand;
        public ICommand FoldCommand => _foldCommand;
        public ICommand ExecuteRaiseCommand => _executeRaiseCommand;
        public ICommand CancelCommand => _cancelCommand;
        public ICommand ReturnToMenuCommand => _returnToMenuCommand;
        public ICommand ShowCombinationsCommand => _showCombinationsCommand;
        #endregion

        #region Position Helpers
        public List<bool> DealerPositions => Enumerable.Range(0, Players.Count).Select(IsDealer).ToList();
        public List<bool> SmallBlindPositions => Enumerable.Range(0, Players.Count).Select(IsSmallBlind).ToList();
        public List<bool> BigBlindPositions => Enumerable.Range(0, Players.Count).Select(IsBigBlind).ToList();

        public bool IsDealer(int index) => index == DealerPosition;
        public bool IsSmallBlind(int index) => index == (DealerPosition + 1) % Players.Count;
        public bool IsBigBlind(int index) => index == (DealerPosition + 2) % Players.Count;
        #endregion

        #region Services
        private readonly NavigationService _navigationService;
        public Tournament CurrentTournament { get; }
        public NavigationService NavigationService => _navigationService;
        #endregion

        public GameViewModel(Tournament tournament, NavigationService navigationService)
        {
            CurrentTournament = tournament;
            _navigationService = navigationService;

            _bigBlind = (int)tournament.BigBlind;
            _buyIn = tournament.BuyIn;
            _prize = tournament.Prize;

            PlayerCards = new ObservableCollection<Card>();
            Bot1Cards = new ObservableCollection<Card>();
            Bot2Cards = new ObservableCollection<Card>();
            Bot3Cards = new ObservableCollection<Card>();
            Bot4Cards = new ObservableCollection<Card>();
            CommunityCards = new ObservableCollection<Card>();

            InitializeCommands();
            InitializeManagers();

            _betDisplayTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(BET_DISPLAY_DURATION_SECONDS) };
            _betDisplayTimer.Tick += (s, e) => { CurrentBetDisplay = ""; _betDisplayTimer.Stop(); };

            InitializeGame();
        }

        private void InitializeManagers()
        {
            StateManager = new GameStateManager(this);
            BettingManager = new GameBettingManager(this);
            BotManager = new GameBotManager(this);
            WinnerManager = new GameWinnerManager(this, _navigationService);
            RoundManager = new GameRoundManager(this);
        }

        private void InitializeCommands()
        {
            _betCommand = new RelayCommand(() => BettingManager?.Bet());
            _raiseCommand = new RelayCommand(() => BettingManager?.ShowRaiseMenu());
            _callCommand = new RelayCommand(() => BettingManager?.Call());
            _foldCommand = new RelayCommand(() => BettingManager?.Fold());
            _returnToMenuCommand = new RelayCommand(ReturnToMenu);
            _showCombinationsCommand = new RelayCommand(ShowCombinations);

            _executeRaiseCommand = new RelayCommand<int>(amount =>
            {
                if (amount > 0)
                {
                    BettingManager?.ExecuteRaise(amount);
                }
                IsRaiseMenuVisible = false;
            });
            _cancelCommand = new RelayCommand(() => IsRaiseMenuVisible = false);
        }

        private void InitializeGame()
        {
            _deck = new Deck();
            _deck.Shuffle();

            _players = new List<Player>();

            var factories = new List<IPlayerFactory>
            {
                new HumanPlayerFactory(),
                new BotAggresiveFactory(),
                new BotConservativeFactory(),
                new BotLooseFactory(),
                new BotTightFactory()
            };

            _players = factories
                .Select(f => f.CreatePlayer(_buyIn))
                .ToList();



            PlayerBalance = _buyIn;
            PotSize = 0;
            CurrentBet = 0;
            DealerPosition = 0;
            ShowBlindsInfo = true;
            CanAct = true;

            RoundManager.DealCards();
            StateManager.TransitionToState(new PreFlopState());
            BettingManager.StartBettingRound();
        }

        public void UpdateCallCheckButtonText()
        {
            int amountToCall = CurrentBet - _players[0].CurrentBet;
            CallCheckButtonText = amountToCall == 0 ? "Check" : $"Call ${amountToCall}";
        }

        private void ReturnToMenu()
        {
            _navigationService.NavigateTo<MenuView>(new MenuViewModel(_navigationService));
        }

        private void ShowCombinations()
        {
            var combinationsWindow = new PokerCombinationsWindow();
            combinationsWindow.Show();
        }

        public void TransitionToState(IGameState newState)
        {
            StateManager.TransitionToState(newState);
        }

        public bool IsBettingRoundComplete()
        {
            return StateManager.IsBettingRoundComplete();
        }

        public void PostBlinds()
        {
            RoundManager.PostBlinds();
        }

        public void DealCommunityCards(int count)
        {
            RoundManager.DealCommunityCards(count);
        }

        public void ClearPlayerActions()
        {
            RoundManager.ClearPlayerActions();
        }

        public void EndGame()
        {
            WinnerManager.EndGame();
        }

        public void ShowGameMessage(string message)
        {
            GameMessage = message;
        }

        public Player GetPlayer(int index)
        {
            return _players[index];
        }

        public List<Player> GetActivePlayers()
        {
            return _players.Where(p => !p.IsFolded && p.Balance >= 0).ToList();
        }

        public void SetEndedFlag(bool value)
        {
            _isEnded = value;
        }
    }
}