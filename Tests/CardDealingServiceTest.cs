using System.Collections.Generic;
using NUnit.Framework;
using TarotFr.Infrastructure;
using TarotFr.Api;

namespace TarotFrTests
{
    class CardDealingServiceTest
    {
        private LinkedList<Player> Musketeers(int nb)
        {
            string[] names = new string[]
            {
                "D'Artagnan",
                "Atos",
                "Portos",
                "Aramis",
                "Albert"
            };

            LinkedList<Player> players = new LinkedList<Player>();

            while( nb > 0)
            {
                players.AddFirst(new Player(names[nb-1]));
                nb--;
            }
            return players;
        }

        [TestCase(3, 90, 6)]
        [TestCase(4, 90, 6)]
        [TestCase(5, 90, 3)]
        public void DealerDealsDeckIsCorrect(int nbPlayers, int totalScore, int nbCardDogs)
        {
            LinkedList<Player> players = Musketeers(nbPlayers);               
            TarotTable tarotTable = new TarotTable(true, true, players);
            CardDealingService service = new CardDealingService(players.Count);
            int sumPlayerHands = 0;

            players.First.Value.MakeDealer();
            service.DealsAllCardsFromDeck(tarotTable);           

            foreach (Player player in players)
            {
                sumPlayerHands += player.ScoreInHand();

                Assert.AreEqual(players.First.Value.NbCardsInHand(), player.NbCardsInHand());

                if (player.IsDealer()) { Assert.AreEqual(nbCardDogs, player.NbCardsInDog()); }
                else { Assert.AreEqual(0, player.NbCardsInDog()); }                
            }

            Assert.AreEqual(90, sumPlayerHands, 1);            
        }
    }
}

