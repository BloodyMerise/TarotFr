using NUnit.Framework;
using System.Collections.Generic;
using TarotFr.Infrastructure;
using TarotFr.Api;
using System;

namespace TarotFrTests
{
    public class RoundTest
    {
        private List<Player> GeneratePlayers(int nbPlayers)
        {
            List<Player> allPlayers = new List<Player>() {
                    new Player("Josette"),
                    new Player("Fanfan"),
                    new Player("Ursule"),
                    new Player("Angele"),
                    new Player("Simeon")
            };

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
            Player playerIn = new Player("Josette");
            Player playerOut = new Player("asd");
            Round round = new Round(true, GeneratePlayers(3));

            Assert.True(round.IsPlayerIn(playerIn));
            Assert.False(round.IsPlayerIn(playerOut));
        }

        [TestCase(3, 2)]
        [TestCase(4, 1)]
        [TestCase(5, 4)]
        [TestCase(3, 1)]
        [TestCase(4, 0)]
        [TestCase(4, 2)]
        [TestCase(5, 0)]
        public void RoundFinishWithDealerAndRotateLeft(int nbPlayers, int dealerPosition)
        {
            List<Player> roundPlayers = GeneratePlayers(nbPlayers);
            Round round = new Round(true, roundPlayers);

            roundPlayers[dealerPosition].MakeDealer();
            
            for (int i = dealerPosition + 1; i < nbPlayers; i++)
            {
                Assert.AreEqual(roundPlayers[i].Name, round.NextPlayer().Name);
            }

            for (int i = 0; i < dealerPosition + 1; i++)
            {
                Assert.AreEqual(roundPlayers[i].Name, round.NextPlayer().Name);
            }
        }

        [TestCase(3, 1)]
        [TestCase(4, 2)]
        [TestCase(5, 0)]
        [TestCase(3, 2)]
        [TestCase(4, 1)]
        [TestCase(4, 0)]
        [TestCase(5, 4)]
        public void RoundFinishWithDealerAndRotateRight(int nbPlayers, int dealerPosition)
        {
            List<Player> roundPlayers = GeneratePlayers(nbPlayers);
            Round round = new Round(false, roundPlayers);

            roundPlayers[dealerPosition].MakeDealer();
            
            for (int i = dealerPosition - 1; i >= 0 ; i--)
            {
                Assert.AreEqual(roundPlayers[i].Name, round.NextPlayer().Name);
            }

            for (int i = nbPlayers -1; i > dealerPosition - 1; i--)
            {
                Assert.AreEqual(roundPlayers[i].Name, round.NextPlayer().Name);
            }
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
            roundPlayers[0].MakeDealer();

            for (int i = 0; i < nbTimesPlayed; i++)
            {
                round.NextPlayer();
            }

            Assert.AreEqual(expectedNumberOfRounds, round.RoundNumber);
        }
    }
}
