using NUnit.Framework;
using TarotFr.Domain;
using System.Linq;
using System.Collections.Generic;
using System;

namespace TarotFrTests
{
    public class CardTests
    {
        [SetUp]
        public void Setup()
        {
        }

        static IEnumerable<Card> TestCards(bool onlyBasics, bool onlyOudlers)
        {
            if (onlyBasics)
            {
                yield return new Card("hearts", 3);
                yield return new Card("spades", 14);
                yield return new Card("clubs", 1);
                yield return new Card("diamonds", 10);
            }
            else if (onlyOudlers)
            {
                yield return new Card("trumpers", 1);
                yield return new Card("trumpers", 21);
                yield return new Card("trumpers", 0);
            }
            else
            {
                yield return new Card("hearts", 3);
                yield return new Card("spades", 14);
                yield return new Card("clubs", 1);
                yield return new Card("diamonds", 10);
                yield return new Card("trumPers", 3);
                yield return new Card("truMpers", 1);
                yield return new Card("trUmpers", 21);
                yield return new Card("tRumpers", 0);
                yield return new Card("Trumpers", 10);
                yield return new Card("trumpers", 1);
                yield return new Card("trumpers", 21);
            }
        }

        static IEnumerable<(List<Card>,int score)> TestCardLists()
        {
            TarotDeck dek = new TarotDeck(true,true);

            yield return (new List<Card>() { new Card("hearts", 8) }, 0);
            yield return (new List<Card>() { new Card("trumpers", 1) }, 0);
            yield return (new List<Card>() { new Card("trumpers", 1) , new Card("hearts", 14) }, 0);
            yield return (new List<Card>() { new Card("hearts", 8), new Card("trumpers", 3) , new Card("hearts", 14) }, 5);
            yield return (new List<Card>() { new Card("hearts", 8), new Card("hearts", 14)}, 5);
            yield return (new List<Card>() { new Card("hearts", 8), new Card("trumpers", 14)}, 1);
            yield return (dek.Take(78).Where(x => x.Score() == 0).ToList(), 29); //40 basic + 19 trumpers = 59/2 points
            yield return (dek.Take(78).Where(x => x.Points() == 10).ToList(), 2); //4 basic + 9 trumpers = 5/2 points
            yield return (dek.Take(78).Where(x => x.Score() != 0).ToList(), 0); //cannot count only figures
        }

        [TestCaseSource(nameof(TestCards), new object[] { false, false })]
        [TestCaseSource(nameof(TestCards), new object[] { false, true })]
        public void CanCreateAllCards(Card card)
        {
            Assert.DoesNotThrow(() => new Card(card.getColor(), card.Points()));
        }

        [Test]
        [TestCase("hearts", 15)]
        [TestCase("clubs", 0)]
        [TestCase("trumpers", 22)]
        [TestCase("clubs", -1)]        
        public void CreateOnlyValidCards(string color, int point)
        {            
            Assert.Throws<ArgumentOutOfRangeException> (() => new Card(color, point));
        }

        [TestCase("rewe", -1)]
        public void CardThrowsOnWrongInput(string color, int point)
        {
            Assert.Throws<ArgumentException>(() => new Card(color, point)); //enum only sends ArgumentException
        }

        [Test]
        [TestCase("hearts", 11, false)]
        [TestCase("trumpers", 11, false)]
        [TestCase("spades", 9, false)]
        [TestCase("trumpers", 21, true)]
        [TestCase("trumpers", 1, true)]
        [TestCase("trumpers", 0, true)]
        public void CardDetectsOudlers(string color, int points, bool isOudler)
        {
            Card testCard = new Card(color, points);
            Assert.AreEqual(isOudler, testCard.IsOudler());
        }

        [TestCase("hearts", 11, false)]
        [TestCase("trumpers", 11, true)]
        [TestCase("spades", 9, false)]
        [TestCase("trumpers", 21, true)]
        [TestCase("trumpers", 1, true)]
        [TestCase("trumpers", 0, true)]
        public void CardDetectsTrumpers(string color, int points, bool isTrumper)
        {
            Card testCard = new Card(color, points);
            Assert.AreEqual(isTrumper, testCard.IsTrumper());
        }

        [TestCaseSource(nameof(TestCards), new object[] { false, true })]
        public void OudlerScoreIs5(Card testCard)
        {
            Assert.AreEqual(5, testCard.Score());
        }

        [Test]
        [TestCaseSource(nameof(TestCards), new object[] { true, false })]
        public void BasicScore(Card testCard)
        {
            int score = testCard.Points() - 10;
            score = (score > 0) ? testCard.Points() - 9 : 0;
            Assert.AreEqual(score, testCard.Score());
        }

        [TestCase("trumpers", 11)]
        public void TrumperScore0(string color, int point)
        {
            Card testCard = new Card(color, point);
            Assert.AreEqual(0, testCard.Score());
        }

        [Test]
        public void TarotDeckIsCorrect()
        {
            TarotDeck myTarotDeck = new TarotDeck(true,false);

            Assert.AreEqual(14, myTarotDeck.TakeAll().Where(x => x.getColor() == "hearts").Count());
            Assert.AreEqual(22, myTarotDeck.TakeAll().Where(x => x.getColor() == "trumpers").Count());
            Assert.AreEqual(5, myTarotDeck.TakeAll().Where(x => x.Points() == 1).Count());
            Assert.AreEqual(78, myTarotDeck.TakeAll().Count());            
        }

        [Test]
        public void TarotDeckIsShuffledInPlace()
        {
            TarotDeck myDeck = new TarotDeck(true,false);
            TarotDeck shuffleDeck = new TarotDeck(true,true);    
            
            Assert.AreNotEqual(myDeck.Pop().ToString(), shuffleDeck.Pop().ToString());
        }
        
        [Test]
        [TestCase("trumpers", 1, true)]
        [TestCase("trumpers", 8, true)]
        [TestCase("trumpers", 21, true)]
        [TestCase("trumpers", 0, false)]
        [TestCase("hearts", 13, true)]
        [TestCase("spades", 7, false)]
        public void CheckCardBeatsCard(string color, int points, bool expected)
        {
            Card basic = new Card("hearts", 8);
            Card testCard = new Card(color, points);

            Assert.AreEqual(true, testCard != basic);
            Assert.AreEqual(expected, testCard > basic);
            Assert.AreEqual(!expected, testCard < basic);
        }

        [TestCase("hearts", 8, false)]
        [TestCase("spades", 8, false)]
        public void CardsOfSameStrengthDontWin(string color, int points, bool expected)
        {
            Card basic = new Card("hearts", 8);
            Card testCard = new Card(color, points);

            Assert.AreEqual(expected, testCard > basic);
            Assert.AreEqual(expected, testCard < basic);
            Assert.AreEqual(true, testCard == basic);
            Assert.AreEqual(false, testCard != basic);
        }

        [TestCase("hearts", 14, 5)]
        [TestCase("trumpers", 0, 5)]
        [TestCase("spades", 2, 1)]
        public void CountScoreIsCorrect(string color, int points, int expectedScore)
        {
            Card basic = new Card("hearts", 8);
            Card testCard = new Card(color, points);

            Assert.AreEqual(expectedScore, basic.CountScore(testCard));
        }

        [TestCase("hearts", 14)]
        [TestCase("trumpers", 1)]
        public void CountScoreThrowsWith2HighPointsCards(string color, int points)
        {
            Card testCard = new Card(color, points);
            Card vsCard = new Card("spades", 13);

            Assert.Throws<ArgumentException>(() => testCard.CountScore(vsCard));
        }

        [TestCaseSource(nameof(TestCardLists))]    
        public void CountScoreOnFullDeck((List<Card>,int) coupleTest)
        {
            List<Card> cardsToCount = coupleTest.Item1;
            int expectedScore = coupleTest.Item2;

            Assert.AreEqual(expectedScore, cardsToCount.Score());
        }
    }
}