using System;
using System.Linq;
using System.Threading.Tasks;
using TexasHoldemWPF.Enums;
using TexasHoldemWPF.Models.Entities;

namespace TexasHoldemWPF.ViewModels
{
    public class GameBotManager
    {
        private readonly GameViewModel _context;

        public GameBotManager(GameViewModel context)
        {
            _context = context;
        }

        public async Task StartBotActions()
        {
            _context.IsWaitingForPlayerAction = false;
            bool newRaisesOccurred = false;

            for (int i = 0; i < _context.Players.Count; i++)
            {
                int currentIndex = (GameViewModel.BOT_START_INDEX + i) % _context.Players.Count;
                Player currentPlayer = _context.GetPlayer(currentIndex);

                if (ShouldSkipBot(currentIndex, currentPlayer))
                    continue;

                await Task.Delay(GameViewModel.BOT_ACTION_DELAY_MS);

                if (ShouldCheck(currentPlayer))
                {
                    currentPlayer.LastAction = "Check";
                    continue;
                }

                var action = ((BotPlayer)currentPlayer).MakeDecision(
                    _context.CurrentBet,
                    _context.PotSize,
                    _context.CommunityCards.ToList());

                var result = await ProcessBotAction(currentIndex, currentPlayer, action);

                if (result.NewRaisesOccurred && currentIndex != 0)
                {
                    _context.IsWaitingForPlayerAction = true;
                    _context.CanAct = true;
                    return;
                }
            }

            if (_context.StateManager.IsBettingRoundComplete())
            {
                await Task.Delay(GameViewModel.STATE_TRANSITION_DELAY_MS);
                _context.StateManager.AdvanceGameState();
            }
        }

        private bool ShouldSkipBot(int index, Player player)
        {
            return index == 0 || player.IsFolded || player.Balance <= 0;
        }

        private bool ShouldCheck(Player player)
        {
            int amountToCall = _context.CurrentBet - player.CurrentBet;
            return amountToCall <= 0 && _context.CurrentBet != 0 && player.CurrentBet == _context.CurrentBet;
        }

        private async Task<BotActionResult> ProcessBotAction(int index, Player player, ActionType action)
        {
            int amountToCall = _context.CurrentBet - player.CurrentBet;
            var bot = (BotPlayer)player;

            switch (action)
            {
                case ActionType.Fold:
                    return await HandleBotFold(index, player, bot);

                case ActionType.Call:
                    return HandleBotCall(player, amountToCall, index);

                case ActionType.Raise:
                    return HandleBotRaise(player, bot);

                default:
                    return new BotActionResult { NewRaisesOccurred = false };
            }
        }

        private async Task<BotActionResult> HandleBotFold(int index, Player player, BotPlayer bot)
        {
            if (index == GameViewModel.LAST_BOT_INDEX && _context.PlayerBalance == 0)
                return HandleBotCall(player, _context.CurrentBet - player.CurrentBet, index);

            player.IsFolded = true;
            player.LastAction = "Fold";
            _context.ShowGameMessage(_context.GameMessage + $"\n{bot.Name} folded.");
            await CheckForEarlyWin();
            return new BotActionResult { NewRaisesOccurred = false };
        }

        private BotActionResult HandleBotCall(Player player, int amountToCall, int index)
        {
            if (index == GameViewModel.FIRST_BOT_INDEX && _context.PlayerBalance == 0)
                return HandleBotFoldAsync(player, (BotPlayer)player).Result;

            int callAmount = Math.Min(amountToCall, player.Balance);
            player.Balance -= callAmount;
            _context.PotSize += callAmount;
            player.CurrentBet += callAmount;

            player.LastAction = DetermineCallActionText(callAmount, player.Balance);
            _context.OnPropertyChanged(nameof(GameViewModel.Players));

            return new BotActionResult { NewRaisesOccurred = false };
        }

        private async Task<BotActionResult> HandleBotFoldAsync(Player player, BotPlayer bot)
        {
            player.IsFolded = true;
            player.LastAction = "Fold";
            await CheckForEarlyWin();
            return new BotActionResult { NewRaisesOccurred = false };
        }

        private string DetermineCallActionText(int callAmount, int remainingBalance)
        {
            if (callAmount == 0) return "Check";
            return remainingBalance == 0 ? "All-in" : $"Call ${callAmount}";
        }

        private BotActionResult HandleBotRaise(Player player, BotPlayer bot)
        {
            if (_context.BettingManager.RaiseOccurredInRound)
                return HandleBotCall(player, _context.CurrentBet - player.CurrentBet,
                    _context.Players.IndexOf(player));

            int minRaise = Math.Max(_context.CurrentBet + _context.BigBlindAmount, _context.CurrentBet * 2);
            int raiseAmount = Math.Min(minRaise, player.Balance);

            player.Balance -= raiseAmount;
            _context.PotSize += raiseAmount;
            _context.CurrentBet = player.CurrentBet + raiseAmount;
            player.CurrentBet = _context.CurrentBet;

            _context.BettingManager.SetRaiseOccurred(true);
            player.LastAction = player.Balance == 0 ? "All-in" : $"Raise ${raiseAmount}";
            _context.UpdateCallCheckButtonText();
            _context.OnPropertyChanged(nameof(GameViewModel.Players));

            return new BotActionResult { NewRaisesOccurred = true };
        }

        private async Task CheckForEarlyWin()
        {
            var activePlayers = _context.GetActivePlayers();
            if (activePlayers.Count == 1)
            {
                var winner = activePlayers.First();
                winner.Balance += _context.PotSize;
                if (winner == _context.GetPlayer(0))
                {
                    _context.PlayerBalance = winner.Balance;
                }
                _context.ShowGameMessage($"{winner.Name} wins ${_context.PotSize} (all others folded)");
                await Task.Delay(GameViewModel.EARLY_WIN_DELAY_MS);
                _context.WinnerManager.CheckGameEndCondition();
            }
        }

        private class BotActionResult
        {
            public bool NewRaisesOccurred { get; set; }
        }
    }
}