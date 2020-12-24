using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TarotFr.Api;
using TarotFr.Infrastructure;
using TarotFr.Domain;

namespace TarotFrTests
{
    public class TarotDeckTests
    {
        static IEnumerable<(List<Card>, double)> TestCardLists()
        {
            TarotDeck dek = new TarotDeck(true);
            List<Card> wholeDeck = dek.Pop(DealingRules.MaxCardsInDeck).ToList();

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
            yield return (wholeDeck.Where(x => x.Score() == 0.5).ToList(), 29.5); //40 basic + 19 trumpers = 59/2 points
            yield return (wholeDeck.Where(x => x.Points() == 10).ToList(), 2.5); // 4 basic + 1 trumper
            yield return (wholeDeck.Where(x => x.Score() != 0).ToList(), 91); //Chelem/Slam
            yield return (wholeDeck.ToList(), CardCountingRules.MaxScore); //Chelem/Slam
        }

        [Test]
        public void PoppingMoreCardsThanAvailableReturnsOnlyAvailableCards()
        {
            TarotDeck deck = new TarotDeck(false);
            Assert.AreEqual(DealingRules.MaxCardsInDeck,deck.Pop(DealingRules.MaxCardsInDeck + 10 ).Count());            
        }

        [Test]
        public void Pops0CardsWhenEmpty()
        {
            TarotDeck deck = new TarotDeck(false);
            IEnumerable<Card> tst = deck.Pop(DealingRules.MaxCardsInDeck);
            tst.Score(); // consume the enumerable

            Assert.IsTrue(deck.IsEmpty());
            Assert.AreEqual(0, deck.Pop(12).Count());
        }

        [Test]
        public void AllCardsInDeckAreDifferent()
        {
            TarotDeck deck = new TarotDeck(false);
            int nbCardsInDeck = deck.NbCardsInDeck();
            IEnumerable<Card> allCards = deck.Pop(nbCardsInDeck);

            Assert.AreEqual(nbCardsInDeck, allCards.Distinct().Count());
        }

        [TestCase(-100)]
        [TestCase(0)]
        [TestCase(DealingRules.MaxCardsInDeck)]        
        public void PoppingInvalidNbCardsDoesNotThrow(int nbCards)
        {
            TarotDeck deck = new TarotDeck(false);
            IEnumerable<Card> tst = deck.Pop(nbCards + 1);

            Assert.DoesNotThrow(() => tst.Score());
        }

        [TestCase(-100)]
        [TestCase(0)]
        public void PoppingInvalidNbCardsDoesNotRemoveFromDeck(int nbCards)
        {
            TarotDeck deck = new TarotDeck(false);
            IEnumerable<Card> tst = deck.Pop(nbCards);
            tst.Score();

            Assert.AreEqual(DealingRules.MaxCardsInDeck, deck.NbCardsInDeck());
        }

        [Test]
        public void NewTarotDeckHasCorrectNumberOfCards()
        {
            TarotDeck deck = new TarotDeck(false);
            Assert.AreEqual(DealingRules.MaxCardsInDeck, deck.Pop(DealingRules.MaxCardsInDeck).Count());
        }

        [Test]
        public void TarotDeckIsCorrect()
        {
            TarotDeck deck = new TarotDeck(false);
            List<Card> allCards = deck.Pop(DealingRules.MaxCardsInDeck).ToList();

            Assert.AreEqual(14, allCards.Where(x => x.getColorAsString() == "hearts").Count());
            Assert.AreEqual(22, allCards.Where(x => x.getColorAsString() == "trumpers").Count());
            Assert.AreEqual(5, allCards.Where(x => x.Points() == 1).Count());
            Assert.AreEqual(78, allCards.Count());
        }

        [Test] //how to make this test always successful (deterministic ?) ?
        public void TarotDeckIsShuffledInPlace()
        {
            TarotDeck unshuffledDeck = new TarotDeck(false);
            TarotDeck shuffledDeck = new TarotDeck(true);

            Assert.AreNotEqual(unshuffledDeck.Pop(), shuffledDeck.Pop());            
        }

        [TestCaseSource(nameof(TestCardLists))]
        public void CountScoreOnFullDeck((List<Card>, double) coupleTest)
        {
            List<Card> cardsToCount = coupleTest.Item1;
            double expectedScore = coupleTest.Item2;

            Assert.AreEqual(expectedScore, cardsToCount.Score());
        }
    }
}