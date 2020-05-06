using System;
using TarotFr.Api;
using TarotFr.Infrastructure;
using NUnit.Framework;
using System.Collections.Generic;

namespace TarotFrTests
{
    class CardScoreTest
    {
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

        [TestCase("hearts", 14, 4.5)]
        [TestCase("trumpers", 0, 4.5)]
        [TestCase("spades", 2, 0.5)]
        public void CountScoreIsCorrect(string color, int points, double expectedScore)
        {            
            List<Card> testCards = new List<Card>() { new Card(color, points) };
            Assert.AreEqual(expectedScore, testCards.Score());
        }


        [TestCaseSource(nameof(TestCards), new object[] { false, true })]
        public void OudlerScoreIs45(Card testCard)
        {
            Assert.AreEqual(4.5, testCard.Score());
        }

        [Test]
        [TestCaseSource(nameof(TestCards), new object[] { true, false })]
        public void BasicScore(Card testCard)
        {
            List<Card> testCards = new List<Card>() { testCard };

            double score = testCard.Points() - 10;
            score = (score > 0) ? testCard.Points() - 9.5 : 0.5;

            Assert.AreEqual(score, testCard.Score());
        }

        [TestCase("trumpers", 11)]
        public void TrumperScoreIs05(string color, int point)
        {
            List<Card> testCards = new List<Card>() { new Card(color, point) } ;
            Assert.AreEqual(0.5, testCards.Score());
        }
    }
}
