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

        [TestCase(3)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(78)]
        [TestCase(99)]
        public void PlayerSelectsCardsFromDesk(int nbcards)
        {
            Player testPlayer = new Player("Raoul");
            TarotDeck dek = new TarotDeck();
            testPlayer.TakesCards(dek, nbcards);
            Assert.AreEqual(nbcards, testPlayer.NbCards());
        }
    }
}
