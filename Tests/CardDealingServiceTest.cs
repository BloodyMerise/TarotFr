using System.Collections.Generic;
using NUnit.Framework;
using TarotFr.Infrastructure;
using TarotFr.Domain;
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

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void AlwaysDealsCorrectly(int nbPlayers)
        {
            LinkedList<Player> players = Musketeers(nbPlayers);               
            TarotTable tarotTable = new TarotTable(true, true, players);
            CardDealingService service = new CardDealingService(players.Count);
            int totalCardsInHand = 0;

            players.First.Value.MakeDealer();
            service.DealsAllCardsFromDeck(tarotTable);

            foreach (Player player in players)
            {
                totalCardsInHand += player.NbCardsInHand();
                Assert.AreEqual(players.First.Value.NbCardsInHand(), player.NbCardsInHand());
                Assert.AreEqual(0, player.NbCardsInDog());
            }

            Assert.AreEqual(78, totalCardsInHand + tarotTable.CountDog());
            Assert.AreEqual(CardDealingRules.DogMaxCards(nbPlayers), tarotTable.CountDog());
        }
    }
}

