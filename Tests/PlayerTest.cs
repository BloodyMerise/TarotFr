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
        [TestCase(99,78)]
        public void CannotDealInvalidNbCards(int nbcards, int expectedNbCards)
        {
            Player raoul = new Player("Raoul");
            Player georgette = new Player("Georgette");
            TarotTable dek = new TarotTable(true,true);

            raoul.MakeDealer();
            raoul.GivesHandNbCardsFromDeck(dek, nbcards,georgette);

            Assert.AreEqual(expectedNbCards, georgette.NbCardsInHand());
        }

        [Test]
        public void NonDealerCannotDealCards()
        {
            Player maurice = new Player("Maurice");
            Player hugolin = new Player("Hugolin");
            Player marieJeanne = new Player("Marie-Jeanne");
            LinkedList <Player> round = new LinkedList<Player>();            
            TarotTable dek = new TarotTable(true,true);

            round.AddFirst(maurice);
            round.AddFirst(hugolin);
            round.AddFirst(marieJeanne);
            
            maurice.DealsAllCardsFromDeck(dek, round);

            Assert.Zero(maurice.NbCardsInHand());
            Assert.Zero(marieJeanne.NbCardsInHand());
            Assert.Zero(hugolin.NbCardsInHand());
        }

        [Test]
        public void DealerDealsDeckTo3PlayersIsCorrect() {
            LinkedList<Player> round = new LinkedList<Player>();
            round.AddFirst(new Player("Raoul"));
            round.AddFirst(new Player("Georgette"));
            round.AddFirst(new Player("Robert"));            
            TarotTable deck = new TarotTable(true,true);
            int sumPlayerScore = 0;
            Player dealer = round.First();

            dealer.MakeDealer();
            dealer.DealsAllCardsFromDeck(deck, round);            
            sumPlayerScore = dealer.ScoreInDog();

            foreach(Player player in round)
            {
                sumPlayerScore += player.ScoreInHand();
            }

            Assert.AreEqual(90, sumPlayerScore,1);
            Assert.AreEqual(dealer.NbCardsInHand(), round.Last().NbCardsInHand());
            Assert.AreEqual(6, dealer.NbCardsInDog());            
        }

        [Test]
        public void DealerDealsDeckTo4PlayersIsCorrect()
        {
            LinkedList<Player> round = new LinkedList<Player>();
            round.AddFirst(new Player("SophiePetoncule"));
            round.AddFirst(new Player("Hemiplegiane"));
            round.AddFirst(new Player("MrMarcadet"));
            round.AddFirst(new Player("JeanBulbe"));
            TarotTable deck = new TarotTable(true,true);
            int sumPlayerScore = 0;
            Player dealer = round.First();

            dealer.MakeDealer();
            dealer.DealsAllCardsFromDeck(deck, round);
            sumPlayerScore = dealer.ScoreInDog();

            foreach (Player player in round)
            {
                sumPlayerScore += player.ScoreInHand();
            }

            Assert.AreEqual(90, sumPlayerScore, 1);
            Assert.AreEqual(dealer.NbCardsInHand(), round.Last().NbCardsInHand());
            Assert.AreEqual(6, dealer.NbCardsInDog());
        }

        [Test]
        public void DealerDealsDeckTo5PlayersIsCorrect()
        {
            LinkedList<Player> round = new LinkedList<Player>();
            round.AddFirst(new Player("SophiePetoncule"));
            round.AddFirst(new Player("Hemiplegiane"));
            round.AddFirst(new Player("MrMarcadet"));
            round.AddFirst(new Player("Pef"));
            round.AddFirst(new Player("JeanBulbe"));
            TarotTable deck = new TarotTable(true,true);
            int sumPlayerScore = 0;
            Player dealer = round.First();

            dealer.MakeDealer();
            dealer.DealsAllCardsFromDeck(deck, round);
            sumPlayerScore = dealer.ScoreInDog();

            foreach (Player player in round)
            {
                sumPlayerScore += player.ScoreInHand();
            }

            Assert.AreEqual(90, sumPlayerScore, 1);
            Assert.AreEqual(dealer.NbCardsInHand(), round.Last().NbCardsInHand());
            Assert.AreEqual(3, dealer.NbCardsInDog());
        }

        [Test]
        public void PlayerWithWinningContractIsAttacker()
        {
            Assert.Fail();
        }
    }
}
