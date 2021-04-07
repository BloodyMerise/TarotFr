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
                maurice.WonHands.Add(new Card("trumpers", 1));
            }

            _ps.SetTargetScore(maurice);

            Assert.That(maurice.TargetScore, Is.EqualTo(expectedScore));
        }

        [Test]
        public void PlayerWithHandfulIsDetected()
        {
            Player maurice = _ps.CreatePlayer("Maurice", true, true);
            for (int i = 0; i < 8; i++)
            {
                maurice.Hand.Add(new Card("trumpers", 1));
            }
            _ps.AskForHandful(maurice);

            Assert.That(_ps.CanDeclareHandful(maurice),Is.False);
            Assert.That(maurice.HasHandful, Is.False);

            maurice.Hand.Add(new Card("trumpers", 1));
            _ps.AskForHandful(maurice);

            Assert.That(maurice.HasHandful,Is.True);
            Assert.That(_ps.CanDeclareHandful(maurice), Is.True);
        }

        [Test]
        public void PlayerWitMisereIsDetected()
        {
            Player maurice = _ps.CreatePlayer("Maurice", true, true);
            maurice.Hand.Add(new Card("spades", 10));
            maurice.Hand.Add(new Card("diamonds", 10));
            _ps.AskForMisere(maurice);

            Assert.That(_ps.CanDeclareMisere(maurice), Is.True);
            Assert.That(maurice.HasMisere, Is.True);

            maurice.HasMisere = false;
            maurice.Hand.Add(new Card("spades", 14));
            _ps.AskForMisere(maurice);

            Assert.That(_ps.CanDeclareMisere(maurice), Is.False);
            Assert.That(maurice.HasMisere, Is.False);
        }
    }
}
