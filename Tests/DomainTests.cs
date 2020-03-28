using NUnit.Framework;
using TarotFr.Domain;

namespace TarotFrTests
{
    public class DomainTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("spades", 3)]
        [TestCase("spades", 0)]
        [TestCase("spades", -1)]
        [TestCase("spades", null)]
        [TestCase(null, null)]
        public void CardReturnsCardDescription(string color, int points)
        {            
            Card card = new Card(color, points);
            Assert.AreEqual(points + color, card.Show());
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
        }
        [Test]
        [TestCase("hearts", 11, false)]
        [TestCase("trumps", 11, false)]
        [TestCase("spades", 9, false)]
        [TestCase("trumps", 21, true)]
        [TestCase("trumps", 1, true)]
        [TestCase("trumps", 0, true)]
        public void CardDetectsOudlers(string color, int points, bool isOudler)
        {
            Card testCard = new Card(color, points);
            Assert.AreEqual(isOudler, testCard.IsOudler());
        }

        [Test]
        [TestCase("hearts", 11, false)]
        [TestCase("trumps", 11, true)]
        [TestCase("spades", 9, false)]
        [TestCase("trumps", 21, true)]
        [TestCase("trumps", 1, true)]
        [TestCase("trumps", 0, true)]
        public void CardDetectsTrumpers(string color, int points, bool isTrumper)
        {
            Card testCard = new Card(color, points);
            Assert.AreEqual(isTrumper, testCard.IsTrumper());
        }

        [Test]
        public void BasicCardFaceValueCapsAt14()
        {
            int faceValue = 15;
            Assert.IsNull(new Card("hearts", faceValue));
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
        */
    }
}