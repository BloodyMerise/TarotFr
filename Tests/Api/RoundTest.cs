using NUnit.Framework;
using System.Collections.Generic;
using TarotFr.Domain;
using TarotFr.Api;
using System;

namespace TarotFrTests
{
    public class RoundTest
    {
        PlayerService _ps = new PlayerService();

        static IEnumerable<Tuple<int,int>> TestPlayersNbAndPosition()
        {
            DealingRules dr = new DealingRules();
            foreach(int nbPlayer in dr.PossibleNbPlayers)
            {
                for(int i = 0; i < nbPlayer; i++)
                {
                    yield return new Tuple<int,int> (nbPlayer, i);
                }                
            }
        }
        private List<Player> GeneratePlayers(int nbPlayers)
        {
            var playerNames = new List<string> { 
                    "Josette",
                    "Fanfan",
                    "Ursule",
                    "Angele",
                    "Simeon"
            };

            var allPlayers = _ps.CreatePlayers(playerNames) as List<Player>;
            
            return allPlayers.GetRange(0, nbPlayers);
        }

        [Test]
        public void RoundWithoutPlayersThrows()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() => new Round(true, null));
        }

        [Test]
        public void CanFindPlayer()
        {
            Player playerIn = _ps.CreatePlayer("Josette", false, false);
            Player playerOut = _ps.CreatePlayer("JoseADaewq", false, false);
            Round round = new Round(true, GeneratePlayers(3));

            Assert.True(round.IsPlayerIn(playerIn));
            Assert.False(round.IsPlayerIn(playerOut));
        }

        [TestCaseSource(nameof(TestPlayersNbAndPosition))]
        public void RoundFinishWithDealerAndRotateLeft(Tuple<int,int> input)
        {
            int nbPlayers = input.Item1;
            int dealerPosition = input.Item2;

            List<Player> roundPlayers = GeneratePlayers(nbPlayers);
            Round round = new Round(true, roundPlayers);

            _ps.MakeDealer(roundPlayers[dealerPosition]);
            
            for (int i = dealerPosition + 1; i < nbPlayers; i++)
            {
                Assert.AreEqual(roundPlayers[i].Name, round.NextPlayer().Name);
            }

            for (int i = 0; i < dealerPosition + 1; i++)
            {
                Assert.AreEqual(roundPlayers[i].Name, round.NextPlayer().Name);
            }
        }

        [TestCaseSource(nameof(TestPlayersNbAndPosition))]
        public void RoundFinishWithDealerAndRotateRight(Tuple<int, int> input)
        {
            int nbPlayers = input.Item1;
            int dealerPosition = input.Item2;

            List<Player> roundPlayers = GeneratePlayers(nbPlayers);
            Round round = new Round(true, roundPlayers);

            _ps.MakeDealer(roundPlayers[dealerPosition]);

            //for (int i = dealerPosition - 1; i >= 0 ; i--)
            for (int i = dealerPosition + 1; i < nbPlayers; i++)
            {
                Assert.AreEqual(roundPlayers[i].Name, round.NextPlayer().Name);
            }

            //for (int i = nbPlayers -1; i > dealerPosition - 1; i--)
            for (int i = 0; i < dealerPosition + 1; i++)
            {
                Assert.AreEqual(roundPlayers[i].Name, round.NextPlayer().Name);
            }
        }

        [Test]
        public void RoundRotatesRightAndLeftCheck()
        {
            List<Player> roundPlayers = GeneratePlayers(3);
            _ps.MakeDealer(roundPlayers[2]);
            Round roundLeft = new Round(true, roundPlayers);
            Round roundRight = new Round(false, roundPlayers);

            string nextNameWhenLeft = roundLeft.NextPlayer().Name;
            string nextNameWhenRight = roundRight.NextPlayer().Name;

            Assert.That(nextNameWhenLeft, Is.Not.EqualTo(nextNameWhenRight));
            Assert.That(nextNameWhenLeft, Is.EqualTo("Josette"));
            Assert.That(nextNameWhenRight, Is.EqualTo("Fanfan"));
        }

        [TestCase(3, 5, 1)]
        [TestCase(4, 4, 1)]
        [TestCase(4, 7, 1)]
        [TestCase(4, 0, 0)]
        [TestCase(5, 1, 0)]
        public void RoundNumberIsCorrect(int nbPlayers, int nbTimesPlayed, int expectedNumberOfRounds)
        {
            List<Player> roundPlayers = GeneratePlayers(nbPlayers);
            Round round = new Round(false, roundPlayers);
            _ps.MakeDealer(roundPlayers[0]);

            for (int i = 0; i < nbTimesPlayed; i++)
            {
                round.NextPlayer();
            }

            Assert.AreEqual(expectedNumberOfRounds, round.RoundNumber);
        }
    }
}
