using NUnit.Framework;
using TarotFr.Api;
using TarotFr.Infrastructure;
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
        [TestCase(99,78)]
        public void CannotDealInvalidNbCards(int nbcards, int expectedNbCards)
        {
            Player raoul = new Player("Raoul");
            Player georgette = new Player("Georgette");
            LinkedList<Player> players = new LinkedList<Player>();

            players.AddFirst(raoul);
            players.AddFirst(georgette);

            TarotTable dek = new TarotTable(true,true, players);

            raoul.MakeDealer();
            raoul.DealsCard(dek.Pop(nbcards), georgette);

            Assert.AreEqual(expectedNbCards, georgette.NbCardsInHand());
        }

        [Test]
        public void NonDealerCannotDealCards()
        {
            Player maurice = new Player("Maurice");
            Player hugolin = new Player("Hugolin");
            Player colette = new Player("Colette");
            LinkedList <Player> players = new LinkedList<Player>();
            players.AddFirst(maurice);
            players.AddFirst(hugolin);        
            players.AddFirst(colette);

            TarotTable dek = new TarotTable(true,true, players);
                        
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
