using NUnit.Framework;
using TarotFr.Api;
using TarotFr.Domain;
using TarotFr.Infrastructure;
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
        
        [TestCase(0,56)]
        [TestCase(1,51)]
        [TestCase(2,41)]
        [TestCase(3,36)]
        public void TargetScoreIsCorrectForPlayer(int oudlersQuantity, int expectedScore)
        {
            Player maurice = _ps.CreatePlayer("Maurice", true, true);
            for(int i = 0; i < oudlersQuantity; i++)
            {
                maurice.Hand.Add(new Card("trumpers", 1));
            }

            _ps.SetTargetScore(maurice);

            Assert.That(maurice.TargetScore, Is.EqualTo(expectedScore));
        }
    }
}
