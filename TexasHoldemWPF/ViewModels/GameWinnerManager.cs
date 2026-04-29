using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TexasHoldemWPF.Enums;
using TexasHoldemWPF.Models.Entities;
using TexasHoldemWPF.Models.Utilities;
using TexasHoldemWPF.Resources;
using TexasHoldemWPF.Views;

namespace TexasHoldemWPF.ViewModels
{
    public class GameWinnerManager
    {
        private readonly GameViewModel _context;
        private readonly NavigationService _navigationService;

        public GameWinnerManager(GameViewModel context, NavigationService navigationService)
        {
            _context = context;
            _navigationService = navigationService;
        }

        public async void EndGame()
        {
            _context.ShowAllCards = true;
            await Task.Delay(GameViewModel.SHOWDOWN_DELAY_MS);

            var activePlayers = _context.GetActivePlayers();
            var communityCards = _context.CommunityCards.ToList();

            if (activePlayers.Count == 1)
            {
                AwardSingleWinner(activePlayers.First());
                return;
            }

            if (_context.PlayerBalance == 0)
            {
                CheckGameEndCondition();
                return;
            }

            var winners = DetermineWinners(activePlayers, communityCards);
            AwardWinners(winners);
            CheckGameEndCondition();
        }

        private void AwardSingleWinner(Player winner)
        {
            winner.Balance += _context.PotSize;
            if (winner == _context.GetPlayer(0))
                _context.PlayerBalance = winner.Balance;
            _context.ShowGameMessage($"{winner.Name} wins ${_context.PotSize} (all others folded)");
            CheckGameEndCondition();
        }

        private List<PlayerEvaluation> DetermineWinners(List<Player> players, List<Card> communityCards)
        {
            var evaluations = players
                .Select(p => new PlayerEvaluation
                {
                    Player = p,
                    Evaluation = HandEvaluator.EvaluateHand(communityCards, p.Hand)
                })
                .OrderByDescending(x => x.Evaluation.Rank)
                .ThenByDescending(x => GetKickerValue(x.Evaluation.Kickers))
                .ToList();

            var bestEvaluation = evaluations.First().Evaluation;
            return evaluations
                .Where(x => CompareEvaluations(x.Evaluation, bestEvaluation) == 0)
                .ToList();
        }

        private void AwardWinners(List<PlayerEvaluation> winners)
        {
            if (winners.Count == 1)
            {
                AwardSingleWinner(winners.First());
            }
            else
            {
                AwardSplitPot(winners);
            }
        }

        private void AwardSingleWinner(PlayerEvaluation winner)
        {
            winner.Player.Balance += _context.PotSize;
            if (winner.Player == _context.GetPlayer(0))
                _context.PlayerBalance = winner.Player.Balance;
            _context.ShowGameMessage($"{winner.Player.Name} wins ${_context.PotSize} with {winner.Evaluation.Rank}");
        }

        private void AwardSplitPot(List<PlayerEvaluation> winners)
        {
            int splitAmount = _context.PotSize / winners.Count;
            int remainder = _context.PotSize % winners.Count;

            foreach (var winner in winners)
            {
                int awardAmount = splitAmount + (winner == winners.First() ? remainder : 0);
                winner.Player.Balance += awardAmount;
                if (winner.Player == _context.GetPlayer(0))
                    _context.PlayerBalance = winner.Player.Balance;
            }

            string winnersNames = string.Join(" and ", winners.Select(x => x.Player.Name));
            _context.ShowGameMessage($"Split pot! {winnersNames} each win ${splitAmount} with {winners.First().Evaluation.Rank}");
        }

        public void DetermineWinnerAfterFold()
        {
            var activePlayers = _context.Players.Where(p => !p.IsFolded).ToList();

            if (_context.CommunityCards.Count < 5)
            {
                AwardUncalledBet(activePlayers);
            }
            else
            {
                AwardBestHandAfterFold(activePlayers);
            }

            _context.RoundManager.StartNewRound();
        }

        private void AwardUncalledBet(List<Player> activePlayers)
        {
            var lastAggressor = FindLastAggressor();
            lastAggressor.Balance += _context.PotSize;
            _context.ShowGameMessage($"{lastAggressor.Name} wins ${_context.PotSize} (uncalled bet)");
        }

        private void AwardBestHandAfterFold(List<Player> activePlayers)
        {
            var evaluations = activePlayers
                .Select(p => new PlayerEvaluation
                {
                    Player = p,
                    Evaluation = HandEvaluator.EvaluateHand(_context.CommunityCards.ToList(), p.Hand)
                })
                .OrderByDescending(x => x.Evaluation.Rank)
                .ThenByDescending(x => x.Evaluation.BestHand.Max(c => c.Rank))
                .ToList();

            var winner = evaluations.First();
            winner.Player.Balance += _context.PotSize;
            _context.ShowGameMessage($"{winner.Player.Name} wins ${_context.PotSize} with {winner.Evaluation.Rank}");
        }

        private Player FindLastAggressor()
        {
            return _context.Players
                .Where(p => !p.IsFolded)
                .OrderByDescending(p => p.CurrentBet)
                .FirstOrDefault() ?? _context.GetPlayer(1);
        }

        public void CheckGameEndCondition()
        {
            if (_context.GetPlayer(0).Balance <= 0)
            {
                _navigationService?.NavigateTo<LoseView>(new LoseViewModel(_navigationService));
                return;
            }

            if (_context.Players.Where(p => p != _context.GetPlayer(0)).All(p => p.Balance <= 0))
            {
                _navigationService?.NavigateTo<WinView>(new WinViewModel(_navigationService, _context.PrizeAmount));
                return;
            }

            _context.RoundManager.StartNewRound();
        }

        private static long GetKickerValue(List<Card> kickers)
        {
            return kickers
                .OrderByDescending(k => k.Rank)
                .Aggregate(0L, (acc, card) => acc * 100 + (int)card.Rank);
        }

        private int CompareEvaluations(HandEvaluator.HandEvaluationResult x, HandEvaluator.HandEvaluationResult y)
        {
            if (x.Rank != y.Rank) return x.Rank.CompareTo(y.Rank);
            for (int i = 0; i < Math.Min(x.Kickers.Count, y.Kickers.Count); i++)
                if (x.Kickers[i].Rank != y.Kickers[i].Rank)
                    return x.Kickers[i].Rank.CompareTo(y.Kickers[i].Rank);
            return 0;
        }

        private class PlayerEvaluation
        {
            public Player Player { get; set; }
            public HandEvaluator.HandEvaluationResult Evaluation { get; set; }
        }
    }
}