using NUnit.Framework;
using System.Collections.Generic;
using TarotFr.Infrastructure;
using TarotFr.Api;
using System;

namespace TarotFrTests
{
    public class RoundTest
    {
        private LinkedList<Player> GeneratePlayers() {
            Player player1 = new Player("Josette");
            Player player2 = new Player("Fanfan");
            Player player3 = new Player("Ursule");
            LinkedList<Player> players = new LinkedList<Player>();
            players.AddLast(player1);
            players.AddLast(player2);
            players.AddLast(player3);

            player2.MakeDealer();

            return players;
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
            Round round = new Round(true, GeneratePlayers());

            Assert.True(round.IsPlayerIn(playerIn));
            Assert.False(round.IsPlayerIn(playerOut));
        }

        [TestCase(false,"Josette")]
        [TestCase(true,"Ursule")]
        public void RoundIsGoingInTheRightDirection(bool isLeft,string nextPlayerName)
        {                        
            Round round = new Round(isLeft, GeneratePlayers());            
            Player dealer = round.FindDealer();

            Assert.AreEqual(nextPlayerName, round.NextPlayer(dealer).Name);
        }
    }
}
