using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TarotFr.Api;
using TarotFr.Domain;
using TarotFr.Infrastructure;

namespace TarotFrTests.Api
{
    class ScoringServiceTests
    {
        PlayerService _ps = new PlayerService();
        BettingService bettingService = new BettingService();
        private List<string> Musketeers()
        {
            return new List<string>
            {
                "D'Artagnan",
                "Atos",
                "Portos",
                "Aramis",
                "Albert"
            };
        }

        static IEnumerable<Tuple<string,int>> ContractScores()
        {
            foreach(var contract in Enum.GetValues(typeof(Contract.Contracts)))
            {
                yield return new Tuple<string, int>(contract.ToString(), (int)contract);
            }
        }


        [TestCase(0, 56)]
        [TestCase(1, 51)]
        [TestCase(2, 41)]
        [TestCase(3, 36)]
        public void TargetScoreIsCorrectForPlayer(int oudlersQuantity, int expectedScore)
        {
            List<Player> players = new List<Player>();
            players.AddRange(_ps.CreatePlayers(Musketeers()));
            ScoringService scoringService = new ScoringService(players, bettingService);
            players.First().Attacker = true;
            for (int i = 0; i < oudlersQuantity; i++)
            {
                players.First().WonHands.Add(new Card("trumpers", 1));
            }

            var targetScore = scoringService.GetTargetScore(players);

            Assert.That(targetScore, Is.EqualTo(expectedScore));
        }

        [TestCaseSource(nameof(ContractScores))]
        public void ContractScoreIsCorrect(Tuple<string,int> testedContract)
        {            
            Contract contract = new Contract(testedContract.Item1);
            List<Player> players = new List<Player>();
            players.AddRange(_ps.CreatePlayers(Musketeers()));
            ScoringService scoringService = new ScoringService(players, bettingService);

            var score = scoringService.GetContractScore(contract);

            Assert.That(score, Is.EqualTo(testedContract.Item2));
        }

    }
}
