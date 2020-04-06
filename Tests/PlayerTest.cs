using NUnit.Framework;
using TarotFr.Domain;
using System.Linq;
using System.Collections.Generic;

namespace TarotFrTests
{
    public class PlayerTest
    {
        [SetUp]
        public void Setup()
        {            
        }

        [TestCase(3,3)]
        [TestCase(-1,0)]
        [TestCase(0,0)]
        [TestCase(78,78)]
        [TestCase(99,0)]
        public void CannotDealInvalidNbCards(int nbcards, int expectedNbCards)
        {
            Player raoul = new Player("Raoul");
            Player georgette = new Player("Georgette");
            TarotDeck dek = new TarotDeck(true,true);

            raoul.MakeDealer();
            raoul.DealsCards(dek, nbcards,georgette);

            Assert.AreEqual(expectedNbCards, georgette.NbCards());
        }

        [Theory]
        public void NonDealerCannotDealCards(bool isDealer)
        {
            Player maurice = new Player("Maurice");
            TarotDeck dek = new TarotDeck(true,true);
            if (isDealer) maurice.MakeDealer();

            maurice.DealsCards(dek, 3, maurice);

            if (isDealer) Assert.Positive(maurice.NbCards());
            else Assert.Zero(maurice.NbCards());            
        }
    }
}
