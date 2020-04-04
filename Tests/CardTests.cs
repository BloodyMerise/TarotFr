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

        [TestCaseSource(nameof(TestCards), new object[] { false, false })]
        [TestCaseSource(nameof(TestCards), new object[] { false, true })]
        public void CanCreateAllCards(Card card)
        {
            Assert.DoesNotThrow(() => new Card(card.Color(), card.Points()));
        }

        [Test]
        [TestCase("hearts", 15)]
        [TestCase("clubs", 0)]
        [TestCase("trumpers", 22)]
        [TestCase("clubs", -1)]
        [TestCase("rewe", -1)]
        public void CreateOnlyValidCards(string color, int point)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Card(color, point));
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

        [TestCaseSource(nameof(TestCards),new object [] {false, true})]
        public void OudlerScoreIs5(Card testCard)
        {
            Assert.AreEqual(5, testCard.Score());
        }

        [Test]
        [TestCaseSource(nameof(TestCards), new object[] { true, false})]
        public void BasicScore(Card testCard)
        {
            int score = testCard.Points() - 10;
            score = (score > 0) ? testCard.Points() - 9 : 0;
            Assert.AreEqual(score, testCard.Score());
        }

        [TestCase("trumpers", 11)]
        public void TrumperScore0(string color,int point)
        {
            Card testCard = new Card(color, point);
            Assert.AreEqual(0, testCard.Score());
        }

        [TestCase("hearts", null, 14, 14)]
        [TestCase("", null, 78, 71)]
        [TestCase("trumpers", null, 22, 15)]
        [TestCase("", 1, 5, 5)]
        public void TarotDeckIsCorrect(string color, int? points,int expectedNbCard,int totalScore)
        {
            TarotDeck myTarotDeck = new TarotDeck();
            Assert.AreEqual(expectedNbCard, myTarotDeck.SelectCards(color,points).Count());
            Assert.AreEqual(78, myTarotDeck.SelectCards().Count());
            //Note that the below scoring is incorrect, this is just arithmetic sum of scores for each card
            Assert.AreEqual(totalScore, myTarotDeck.SelectCards(color, points).Select(x => x.Score()).Sum());
        }

        [TestCase(82)]
        [TestCase(-1)]
        public void TarotDeckPickFailsWithWrongInput(int index)
        {
            TarotDeck dek = new TarotDeck();
            Assert.Throws<ArgumentOutOfRangeException>(() => dek.Pick(index));
        }

        [Test]
        public void TarotDeckIsShuffledInPlace()
        {
            TarotDeck myDeck = new TarotDeck();
            TarotDeck shuffleDeck = new TarotDeck();
            shuffleDeck.Shuffle();
            Assert.AreNotEqual(myDeck.Pick(2).ToString(), shuffleDeck.Pick(2).ToString());
        }

        [Test]
        public void CanSelectListOfCardsFromDeck()
        {
            TarotDeck dek = new TarotDeck();
            List<Card> chosenCards = new List<Card>();

            for(int i=0; i<4 ;i++)
            {
                dek.Shuffle();
                chosenCards.Add(dek.Pick(i));
            }
            Assert.AreEqual(chosenCards, dek.SelectCards(chosenCards));
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
            Card testCard = new Card(color,points);

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
        
        [TestCase("hearts",14,5)]
        [TestCase("trumpers",0,5)]
        [TestCase("spades", 2,1)]
        public void CountScoreIsCorrect(string color, int points,int expectedScore)
        {
            Card basic = new Card("hearts",8);
            Card testCard = new Card(color, points);
            Assert.AreEqual(expectedScore, basic.CountScore(testCard));
        }

        [TestCase("hearts",14)]
        [TestCase("trumpers",1)]
        public void CountScoreThrowsWith2HighPointsCards(string color, int points)
        {
            Card testCard = new Card(color, points);
            Card vsCard = new Card("spades", 13);
            Assert.Throws<ArgumentException>(() => testCard.CountScore(vsCard));
        }
    }
}