using System.Linq;
using System.Windows.Threading;
using TexasHoldemWPF.Models.Entities;
using TexasHoldemWPF.Models.States;

namespace TexasHoldemWPF.ViewModels
{
    public class GameRoundManager
    {
        private readonly GameViewModel _context;

        public GameRoundManager(GameViewModel context)
        {
            _context = context;
        }

        public void StartNewRound()
        {
            if (_context.IsEndedFlag)
                return;

            ResetRoundState();

            _context.CurrentDeck = new Deck();
            _context.CurrentDeck.Shuffle();

            ResetPlayersForNewRound();
            ClearAllCards();

            DealCards();
            _context.StateManager.TransitionToState(new PreFlopState());
            _context.BettingManager.StartBettingRound();
        }

        private void ResetRoundState()
        {
            _context.UpdateCallCheckButtonText();
            _context.ShowAllCards = false;
            ClearBestHandFlags();

            _context.DealerPosition = (_context.DealerPosition + 1) % _context.Players.Count;
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                _context.OnPropertyChanged(nameof(GameViewModel.DealerPositions));
            });

            _context.CanAct = true;
            _context.PotSize = 0;
            _context.CurrentBet = 0;
            _context.ShowBlindsInfo = true;
            ClearPlayerActions();
        }

        private void ClearBestHandFlags()
        {
            foreach (var card in _context.PlayerCards.Concat(_context.Bot1Cards).Concat(_context.Bot2Cards)
                .Concat(_context.Bot3Cards).Concat(_context.Bot4Cards).Concat(_context.CommunityCards))
                if (card != null) card.IsInBestHand = false;
        }

        private void ResetPlayersForNewRound()
        {
            foreach (var player in _context.Players)
            {
                player.ResetForNewRound();
            }
        }

        private void ClearAllCards()
        {
            ClearPlayerCards();
            _context.CommunityCards.Clear();
        }

        private void ClearPlayerCards()
        {
            _context.PlayerCards.Clear();
            _context.Bot1Cards.Clear();
            _context.Bot2Cards.Clear();
            _context.Bot3Cards.Clear();
            _context.Bot4Cards.Clear();
        }

        public void DealCards()
        {
            foreach (var player in _context.Players)
            {
                player.Hand.Clear();
                player.Hand.Add(_context.CurrentDeck.DrawCard());
                player.Hand.Add(_context.CurrentDeck.DrawCard());
            }

            ClearPlayerCards();

            _context.PlayerCards.Add(_context.GetPlayer(0).Hand[0]);
            _context.PlayerCards.Add(_context.GetPlayer(0).Hand[1]);

            _context.Bot1Cards.Add(_context.GetPlayer(1).Hand[0]);
            _context.Bot1Cards.Add(_context.GetPlayer(1).Hand[1]);

            _context.Bot2Cards.Add(_context.GetPlayer(2).Hand[0]);
            _context.Bot2Cards.Add(_context.GetPlayer(2).Hand[1]);

            _context.Bot3Cards.Add(_context.GetPlayer(3).Hand[0]);
            _context.Bot3Cards.Add(_context.GetPlayer(3).Hand[1]);

            _context.Bot4Cards.Add(_context.GetPlayer(4).Hand[0]);
            _context.Bot4Cards.Add(_context.GetPlayer(4).Hand[1]);
        }

        public void PostBlinds()
        {
            int smallBlindPos = (_context.DealerPosition + 1) % _context.Players.Count;
            int bigBlindPos = (_context.DealerPosition + 2) % _context.Players.Count;

            int smallBlindAmount = _context.BigBlindAmount / 2;
            int bigBlindAmount = _context.BigBlindAmount;

            PostBlindAmount(smallBlindPos, smallBlindAmount);
            PostBlindAmount(bigBlindPos, bigBlindAmount);

            _context.PotSize = smallBlindAmount + bigBlindAmount;
            _context.CurrentBet = bigBlindAmount;

            _context.GetPlayer(smallBlindPos).LastAction = "Small Blind";
            _context.GetPlayer(bigBlindPos).LastAction = "Big Blind";

            _context.OnPropertyChanged(nameof(GameViewModel.Players));
        }

        private void PostBlindAmount(int playerIndex, int amount)
        {
            var player = _context.GetPlayer(playerIndex);
            player.Balance -= amount;
            player.CurrentBet = amount;

            if (playerIndex == 0)
                _context.PlayerBalance -= amount;
        }

        public void ClearPlayerActions()
        {
            foreach (var player in _context.Players)
            {
                player.LastAction = "";
            }
        }

        public void DealCommunityCards(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (_context.CurrentDeck.CardsRemaining > 0)
                    _context.CommunityCards.Add(_context.CurrentDeck.DrawCard());
            }
        }
    }
}