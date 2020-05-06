using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TarotFr.Infrastructure;
using TarotFr.Domain;
using TarotFr.Api;

namespace TarotFrTests
{
    class CardDealingServiceTest
    {
        private List<Player> Musketeers(int nb)
        {
            string[] names = new string[]
            {
                "D'Artagnan",
                "Atos",
                "Portos",
                "Aramis",
                "Albert"
            };
           
            List<Player> players = new List<Player>();
            
            while( nb > 0)
            {
                players.Add(new Player(names[nb-1]));
                nb--;
            }
            return players;
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void AlwaysDealsCorrectly(int nbPlayers)
        {
            List<Player> players = Musketeers(nbPlayers);               
            TarotTable tarotTable = new TarotTable(true, true, players);
            CardDealingService service = new CardDealingService(players.Count);

            int totalCardsInHand = 0;

            players[0].MakeDealer();
            service.DealsAllCardsFromDeck(tarotTable);

            foreach (Player player in players)
            {
                totalCardsInHand += player.NbCardsInHand();                
                Assert.AreEqual(players[0].NbCardsInHand(), player.NbCardsInHand()); //all players have same nb cards
                Assert.AreEqual(0, player.NbCardsInDog()); //no player has a dog
            }

            Assert.AreEqual(CardDealingRules.MaxCardsInDeck, totalCardsInHand + tarotTable.CountDog()); //all cards are dealt
            Assert.AreEqual(CardDealingRules.DogMaxCards(nbPlayers), tarotTable.CountDog()); //dog has expected number of cards
        }
    }
}

