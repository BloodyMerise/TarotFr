using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TarotFr.Domain;
using TarotFr.Api;

namespace TarotFrTests
{
    class DealingServiceTest
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

            PlayerService ps = new PlayerService();
            var players = ps.CreatePlayers(names.Take(nb));
                       
            return players as List<Player>;
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void CheckNbCardsInPlayersHandsAndDogIsCorrect(int nbPlayers)
        {
            List<Player> players = Musketeers(nbPlayers);
            TarotTable tarotTable = new TarotTable(true, true, players);
            DealingService dealingService = new DealingService(tarotTable);
            PlayerService playerService = new PlayerService();
            DealingRules rules = new DealingRules();

            int totalCardsInHand = 0;
            playerService.MakeDealer(players.First());
            dealingService.DealsAllCardsFromDeck();

            foreach (Player player in players)
            {
                totalCardsInHand += playerService.CountCardsInHand(player);
                Assert.AreEqual(players[0].Hand.Count(), player.Hand.Count()); //all players have same nb cards
                Assert.IsNull(player.WonHands); //no player has a dog
            }

            Assert.AreEqual(DealingRules.MaxCardsInDeck, totalCardsInHand + tarotTable.CountDog()); //all cards are dealt
            Assert.AreEqual(rules.DogMaxCards(nbPlayers), tarotTable.CountDog()); //dog has expected number of cards
        }

        [Test]
        public void AfterDealingRoundNumberIsZero()
        {
            List<Player> players = Musketeers(5);
            TarotTable tarotTable = new TarotTable(true, true, players);
            DealingService dealingService = new DealingService(tarotTable);
            PlayerService playerService = new PlayerService();
            DealingRules rules = new DealingRules();
            
            playerService.MakeDealer(players.First());
            dealingService.DealsAllCardsFromDeck();

            Assert.That(tarotTable.GetRoundNumber() == 0);
        }
    }
}

