using NUnit.Framework;
using TarotFr.Api;
using TarotFr.Domain;
using System.Linq;

namespace TarotFrTests
{
    public class PlayerTest
    {
        PlayerService _ps = new PlayerService();

        [TestCase(3,3)]
        [TestCase(0,0)]
        [TestCase(78,78)]
        [TestCase(int.MaxValue, 78)]
        [TestCase(int.MinValue, 0)]
        public void CannotDealInvalidNbCards(int nbcards, int expectedNbCards)
        {
            Player raoul = _ps.CreatePlayer("Raoul",false,false);
            Player georgette = _ps.CreatePlayer("Georgette", false, false);

            TarotDeck dek = new TarotDeck(false);

            _ps.MakeDealer(raoul);
            _ps.DealsCards(dek.Pop(nbcards), georgette);

            Assert.AreEqual(expectedNbCards, georgette.Hand.Count());
        }        
    }
}
