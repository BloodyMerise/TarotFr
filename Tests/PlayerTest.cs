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
            TarotDeck dek = new TarotDeck(true);

            raoul.MakeDealer();
            raoul.GivesHandNbCardsFromDeck(dek, nbcards,georgette);

            Assert.AreEqual(expectedNbCards, georgette.NbCardsInHand());
        }

        [Theory]
        public void NonDealerCannotDealCards(bool isDealer)
        {
            Player maurice = new Player("Maurice");
            LinkedList<Player> round = new LinkedList<Player>();            
            TarotDeck dek = new TarotDeck(true);

            round.AddFirst(maurice);
            if (isDealer) maurice.MakeDealer();

            maurice.DealsAllCardsFromDeck(dek, 3, round,6);

            if (isDealer) Assert.Positive(maurice.NbCardsInHand());
            else Assert.Zero(maurice.NbCardsInHand());            
        }

        [Test]
        public void DealerDealsDeckTo3PlayersIsCorrect() {
            LinkedList<Player> round = new LinkedList<Player>();
            round.AddFirst(new Player("Raoul"));
            round.AddFirst(new Player("Georgette"));
            round.AddFirst(new Player("Robert"));
            
            TarotDeck deck = new TarotDeck(true);                        
            int nbCards = 3;
            int sumPlayerScore = 0;
            int limitDog = 6;
            Player dealer = round.First();

            dealer.MakeDealer();
            dealer.DealsAllCardsFromDeck(deck, nbCards, round, limitDog);
            
            sumPlayerScore = dealer.ScoreInDog();

            foreach(Player player in round)
            {
                sumPlayerScore += player.ScoreInHand();
            }

            Assert.AreEqual(90, sumPlayerScore,1);
            Assert.AreEqual(dealer.NbCardsInHand(), round.Last().NbCardsInHand());
            Assert.AreEqual(6, dealer.NbCardsInDog());            
        }
    }
}
