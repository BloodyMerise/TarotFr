using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TarotFr.Domain;
using TarotFr.Api;
using TarotFr.Infrastructure;
using System;

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
        public void CheckNbCardsInPlayersHandsAndAsideIsCorrect(int nbPlayers)
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
                Assert.IsNull(player.WonHands); //no player has a aside
            }

            Assert.AreEqual(DealingRules.MaxCardsInDeck, totalCardsInHand + tarotTable.CountAside()); //all cards are dealt
            Assert.AreEqual(rules.AsideMaxCards(nbPlayers), tarotTable.CountAside()); //aside has expected number of cards
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

        [Test]
        public void FivePlayersAttackerCanCallHimself()
        {
            List<Player> players = Musketeers(5);
            TarotTable tarotTable = new TarotTable(true, true, players);
            DealingService dealingService = new DealingService(tarotTable);
            PlayerService playerService = new PlayerService();
            var kings = new List<object> {
                new Card("hearts",14),
                new Card("diamonds",14),
                new Card("clubs",14),
                new Card("spades",14)
            };
            var attacker = players.First();
            attacker.Attacker = true;
            attacker.Hand.AddRange(kings);
            playerService.MakeDealer(attacker);            

            var calledPlayer = dealingService.AttackerCallsKing(attacker);

            Assert.That(calledPlayer.Name, Is.EqualTo(attacker.Name));
        }

        [Test]
        public void FivePlayersOnlyAttackerCanCallKing()
        {
            List<Player> players = Musketeers(5);
            TarotTable tarotTable = new TarotTable(true, true, players);
            DealingService dealingService = new DealingService(tarotTable);

            var defender = players.First();
            defender.Attacker = false;
            defender.Dealer = true;

            Assert.Throws<ArgumentException>(() => dealingService.AttackerCallsKing(defender));
        }

        [Test]
        public void FivePlayersCalledKingInAsideOneAttacker()
        {
            List<Player> players = Musketeers(5);
            TarotTable tarotTable = new TarotTable(true, true, players);
            DealingService dealingService = new DealingService(tarotTable);
            PlayerService playerService = new PlayerService();
            DealingRules rules = new DealingRules();

            players.First().Dealer = true;
            players.First().Attacker = true;

            Assert.Fail("Use Mock you fool");
        }
    }
}

