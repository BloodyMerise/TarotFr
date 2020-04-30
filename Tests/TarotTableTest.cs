using NUnit.Framework;
using TarotFr.Api;
using TarotFr.Infrastructure;
using System.Linq;
using System.Collections.Generic;

namespace TarotFr.Tests
{
    class TarotTableTest
    {
        static IEnumerable<(List<Card>, double)> TestCardLists()
        {
            TarotTable dek = new TarotTable(true, true, new LinkedList<Player>());

            yield return (new List<Card>() { new Card("hearts", 8) }, 0.5);
            yield return (new List<Card>() { new Card("trumpers", 1) }, 4.5);
            yield return (new List<Card>() { new Card("trumpers", 1), new Card("hearts", 14) }, 9); // -> traditionnally not possible
            yield return (new List<Card>() { new Card("hearts", 8), new Card("trumpers", 3), new Card("hearts", 14) }, 5.5);
            yield return (new List<Card>() { new Card("hearts", 8), new Card("trumpers", 21), new Card("hearts", 14) }, 9.5);
            yield return (new List<Card>() { new Card("hearts", 8), new Card("clubs", 8), new Card("trumpers", 21), new Card("hearts", 14) }, 10);
            yield return (new List<Card>() { new Card("hearts", 8), new Card("clubs", 8), new Card("trumpers", 21), new Card("hearts", 14), new Card("clubs", 14) }, 14.5);
            yield return (new List<Card>() { new Card("hearts", 8), new Card("clubs", 8), new Card("trumpers", 21), new Card("hearts", 14), new Card("diamonds", 14), new Card("clubs", 14) }, 19);
            yield return (new List<Card>() { new Card("hearts", 8), new Card("clubs", 8), new Card("trumpers", 21), new Card("hearts", 14), new Card("diamonds", 14), new Card("clubs", 14), new Card("spades", 14) }, 23.5);
            yield return (new List<Card>() { new Card("hearts", 8), new Card("hearts", 14) }, 5);
            yield return (new List<Card>() { new Card("hearts", 8), new Card("trumpers", 14) }, 1);
            yield return (dek.Take(78).Where(x => x.Score() == 0.5).ToList(), 29.5); //40 basic + 19 trumpers = 59/2 points
            yield return (dek.Take(78).Where(x => x.Points() == 10).ToList(), 2.5); // 4 basic + 1 trumper
            yield return (dek.Take(78).Where(x => x.Score() != 0).ToList(), 91); //Chelem ?
            yield return (dek.Take(78).ToList(), 91); //Chelem ?
        }

        static IEnumerable<Player> PlayerGenerator(int nbPlayers)
        {
            for(int i=0; i < nbPlayers; i++)
            { 
                yield return new Player("Prosper_" + i.ToString());                
            }            
        }

        private TarotTable CreateTable(int nbPlayers, bool startsLeft, bool needDealer)
        {
            LinkedList<Player> round = new LinkedList<Player>();

            foreach(Player player in PlayerGenerator(nbPlayers))
            {                
                round.AddFirst(player);
            }

            if (needDealer) round.First().MakeDealer();

            return new TarotTable(true, startsLeft, round);
        }

        [Test]
        public void TarotDeckIsCorrect()
        {
            TarotTable myTable = new TarotTable(false,true, new LinkedList<Player>());

            Assert.AreEqual(14, myTable.TakeAll().Where(x => x.getColor() == "hearts").Count());
            Assert.AreEqual(22, myTable.TakeAll().Where(x => x.getColor() == "trumpers").Count());
            Assert.AreEqual(5, myTable.TakeAll().Where(x => x.Points() == 1).Count());
            Assert.AreEqual(78, myTable.TakeAll().Count());
        }

        [Test]
        public void TarotDeckIsShuffledInPlace()
        {
            TarotTable myTable = new TarotTable(false, true, new LinkedList<Player>());
            TarotTable shuffleDeck = new TarotTable(true, true, new LinkedList<Player>());

            Assert.AreNotEqual(myTable.DeckPop().ToString(), shuffleDeck.DeckPop().ToString());
        }

        [TestCaseSource(nameof(TestCardLists))]
        public void CountScoreOnFullDeck((List<Card>, double) coupleTest)
        {
            List<Card> cardsToCount = coupleTest.Item1;
            double expectedScore = coupleTest.Item2;

            Assert.AreEqual(expectedScore, cardsToCount.Score());
        }

        [Test]
        public void RoundNextPlayerChangeAccordingToRoundDirection()
        {            
            TarotTable tableLeft = CreateTable(3, true, true);
            TarotTable tableRight = CreateTable(3, false, true);

            Player dealerLeft = tableLeft.GetRoundDealer();
            Player dealerRight = tableRight.GetRoundDealer();

            Assert.AreNotEqual(tableLeft.NextPlayer(dealerLeft).name, tableRight.NextPlayer(dealerRight).name);
        }
    }
}
