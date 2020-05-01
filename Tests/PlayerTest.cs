using NUnit.Framework;
using TarotFr.Api;
using TarotFr.Infrastructure;
using System.Linq;
using System.Collections.Generic;

namespace TarotFrTests
{
    public class PlayerTest
    {
        [TestCase(3,3)]
        [TestCase(-1,0)]
        [TestCase(0,0)]
        [TestCase(78,78)]
        [TestCase(99,78)]
        public void CannotDealInvalidNbCards(int nbcards, int expectedNbCards)
        {
            Player raoul = new Player("Raoul");
            Player georgette = new Player("Georgette");

            TarotDeck dek = new TarotDeck(false);

            raoul.MakeDealer();
            raoul.DealsCard(dek.Pop(nbcards), georgette);

            Assert.AreEqual(expectedNbCards, georgette.NbCardsInHand());
        }

        [Test]
        public void NonDealerDoesntDealCards()
        {
            Player maurice = new Player("Maurice");
            Player hugolin = new Player("Hugolin");
            Player colette = new Player("Colette");
            TarotDeck dek = new TarotDeck(false);
                        
            maurice.DealsCard(dek.Pop(1), hugolin);
            maurice.DealsCard(dek.Pop(100), colette);

            Assert.Zero(maurice.NbCardsInHand());
            Assert.Zero(colette.NbCardsInHand());
            Assert.Zero(hugolin.NbCardsInHand());
        }
        
        [Test]
        public void PlayerWithWinningContractIsAttacker()
        {
            Assert.Fail();
        }
    }
}
