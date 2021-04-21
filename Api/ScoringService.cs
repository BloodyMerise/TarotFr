using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TarotFr.Domain;
using TarotFr.Infrastructure;

namespace TarotFr.Api
{
    public class ScoringService
    {
        List<Player> _players;
        BettingService _bettingService;
        PlayerService _playerService;
        Dictionary<Player, int> _scores;
        private int difficultyMultiplier = 2;

        public ScoringService(List<Player> players, BettingService bettingService)
        {
            _players = players;
            _bettingService = bettingService;
            _playerService = new PlayerService();
            _scores = new Dictionary<Player, int>();

            players.ForEach(x => _scores.Add(x, 0));
        }

        public void ComputeRoundScores()
        {
            var attackers = _players.Where(x => x.Attacker is true);
            var defenders = _players.Where(x => x.Attacker is true);
            double attackScore = attackers.Sum(x => x.WonHands.Cast<Card>().Score());
            double defenseScore = attackers.Sum(x => x.WonHands.Cast<Card>().Score());
            double targetScore = GetTargetScore(attackers.ToList());
            var winningBet = _bettingService.GetWinningBet();

            if (attackScore > targetScore)
            {
                foreach (var player in attackers)
                {
                    _scores[player] = (int)(attackScore - targetScore) + GetContractScore(winningBet.Value) * difficultyMultiplier;
                }
            }
        }

        public double GetTargetScore(List<Player> players)
        {
            return CardCountingRules.TargetScore(
                                        players
                                        .SelectMany(x => x.WonHands.Cast<Card>())
                                        .Count(x => x.IsOudler()));
        }

        public int GetContractScore(Contract contract)
        {
            foreach (var contractPossiblity in Enum.GetValues(typeof(Contract.Contracts)))
            {
                if(contractPossiblity.ToString() == contract.ToString())
                {
                    return (int)contractPossiblity;
                }
            }
            return 0;
        }
    }
}
