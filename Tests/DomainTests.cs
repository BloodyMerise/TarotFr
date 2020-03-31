using NUnit.Framework;
using TarotFr.Domain;
using System.Linq;
using System.Collections.Generic;
using System;

namespace TarotFrTests
{
    public class DomainTests
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
                Assert.Throws<ArgumentOutOfRangeException>(() => new Card(color, point) );
                    
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

        [Test]
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





       
        /*
        [Test]
        [TestCase("trumpers", 1, true)]
        [TestCase("trumpers", 8, true)]
        [TestCase("trumpers", 21, true)]
        [TestCase("trumpers", 0, false)]
        public void TrumperBeatsBasic(string color, int points, bool expected)
        {
            Card basic = new Card("hearts", 8);
            Card trumper = new Card(color,points);

            Assert.AreEqual(expected, basic.Fight(trumper));
        }
        [Test]
        [TestCase("hearts",9)]
        public void BasicHigherCardWinsLowerCard(string color, int points)
        {
            Card currentCard = new Card("spades", 10);
            Card basic1 = new Card(color, points);
            
            Assert.AreEqual(currentCard, currentCard.Fight(basic1));
            Assert.AreEqual(currentCard, basic1.Fight(currentCard));
        }

        [Test]
        [TestCase("hearts",11)]
        public void BasicLowerCardLosesHigherCard(string color, int points)
        {
            Card currentCard = new Card("spades", 10);
            Card basic1 = new Card(color, points);            

            Assert.AreEqual(basic1, basic1.Fight(currentCard));
            Assert.AreEqual(basic1, currentCard.Fight(basic1));
        }*/
    }
}