using NUnit.Framework;
using TarotFr.Domain;
using System.Linq;
using System.Collections.Generic;

namespace TarotFr.Tests
{
    class TarotTableTest
    {
        static IEnumerable<(List<Card>, decimal)> TestCardLists()
        {
            TarotTable dek = new TarotTable(true, true);

            yield return (new List<Card>() { new Card("hearts", 8) }, 0);
            yield return (new List<Card>() { new Card("trumpers", 1) }, 4);
            yield return (new List<Card>() { new Card("trumpers", 1), new Card("hearts", 14) }, 9); // -> traditionnally not possible
            yield return (new List<Card>() { new Card("hearts", 8), new Card("trumpers", 3), new Card("hearts", 14) }, 5);
            yield return (new List<Card>() { new Card("hearts", 8), new Card("trumpers", 21), new Card("hearts", 14) }, 9);
            yield return (new List<Card>() { new Card("hearts", 8), new Card("clubs", 8), new Card("trumpers", 21), new Card("hearts", 14) }, 10);
            yield return (new List<Card>() { new Card("hearts", 8), new Card("clubs", 8), new Card("trumpers", 21), new Card("hearts", 14), new Card("clubs", 14) }, 14);
            yield return (new List<Card>() { new Card("hearts", 8), new Card("clubs", 8), new Card("trumpers", 21), new Card("hearts", 14), new Card("diamonds", 14), new Card("clubs", 14) }, 19);
            yield return (new List<Card>() { new Card("hearts", 8), new Card("clubs", 8), new Card("trumpers", 21), new Card("hearts", 14), new Card("diamonds", 14), new Card("clubs", 14), new Card("spades", 14) }, 23);
            yield return (new List<Card>() { new Card("hearts", 8), new Card("hearts", 14) }, 5);
            yield return (new List<Card>() { new Card("hearts", 8), new Card("trumpers", 14) }, 1);
            yield return (dek.Take(78).Where(x => x.Score() == 0.5M).ToList(), 29); //40 basic + 19 trumpers = 59/2 points
            yield return (dek.Take(78).Where(x => x.Points() == 10).ToList(), 2);
            yield return (dek.Take(78).Where(x => x.Score() != 0).ToList(), 91); //Chelem ?
            yield return (dek.Take(78).ToList(), 91); //Chelem ?
        }

        [Test]
        public void TarotDeckIsCorrect()
        {
            TarotTable myTarotDeck = new TarotTable(false,true);

            Assert.AreEqual(14, myTarotDeck.TakeAll().Where(x => x.getColor() == "hearts").Count());
            Assert.AreEqual(22, myTarotDeck.TakeAll().Where(x => x.getColor() == "trumpers").Count());
            Assert.AreEqual(5, myTarotDeck.TakeAll().Where(x => x.Points() == 1).Count());
            Assert.AreEqual(78, myTarotDeck.TakeAll().Count());
        }

        [Test]
        public void TarotDeckIsShuffledInPlace()
        {
            TarotTable myDeck = new TarotTable(false, true);
            TarotTable shuffleDeck = new TarotTable(true, true);

            Assert.AreNotEqual(myDeck.DeckPop().ToString(), shuffleDeck.DeckPop().ToString());
        }

        [TestCaseSource(nameof(TestCardLists))]
        public void CountScoreOnFullDeck((List<Card>, decimal) coupleTest)
        {
            List<Card> cardsToCount = coupleTest.Item1;
            decimal expectedScore = coupleTest.Item2;

            Assert.AreEqual(expectedScore, cardsToCount.Score());
        }
    }
}
