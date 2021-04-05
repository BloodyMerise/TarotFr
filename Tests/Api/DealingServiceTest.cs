using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TarotFr.Domain;
using TarotFr.Api;
using TarotFr.Infrastructure;
using System;
using Moq;

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

        private IEnumerable<object> Kings() => new List<object> {
                new Card("hearts",14),
                new Card("diamonds",14),
                new Card("clubs",14),
                new Card("spades",14)
            };

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void CheckNbCardsInPlayersHandsAndAsideIsCorrect(int nbPlayers)
        {
            List<Player> players = Musketeers(nbPlayers);            
            DealingService dealingService = new DealingService(true,players);
            PlayerService playerService = new PlayerService();
            DealingRules rules = new DealingRules();

            int totalCardsInHand = 0;
            playerService.MakeDealer(players.First());
            dealingService.DealsAllCardsFromDeck();

            foreach (Player player in players)
            {
                totalCardsInHand += playerService.CountCardsInHand(player);
                Assert.AreEqual(players[0].Hand.Count(), player.Hand.Count()); //all players have same nb cards
                Assert.IsEmpty(player.WonHands); //no player has an aside
            }

            Assert.AreEqual(DealingRules.MaxCardsInDeck, totalCardsInHand + dealingService.CountAside()); //all cards are dealt
            Assert.AreEqual(rules.AsideMaxCards(nbPlayers), dealingService.CountAside()); //aside has expected number of cards
        }

        [Test]
        public void AfterDealingRoundNumberIsZero()
        {
            List<Player> players = Musketeers(5);            
            DealingService dealingService = new DealingService(true, players);
            PlayerService playerService = new PlayerService();
            DealingRules rules = new DealingRules();
            
            playerService.MakeDealer(players.First());
            dealingService.DealsAllCardsFromDeck();

            Assert.That(dealingService.GetRoundNumber() == 0);
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void SendAsideToWinnerMakesPlayerHaveMoreCardThanOthers(int nbPlayers)
        {
            var players = Musketeers(nbPlayers);
            players.First().Dealer = true;
            DealingService ds = new DealingService(true, players);
            ds.DealsAllCardsFromDeck();
            Player attacker = ds.NextPlayer();
            attacker.Attacker = true;

            ds.SendAsideToPlayerHand(attacker);

            while (ds.GetRoundNumber() == 0)
            {
                Player player = ds.NextPlayer();
                Assert.That(attacker.Hand.Count, Is.GreaterThan(player.Hand.Count));
            }
        }

        [Test]
        public void AsideCannotContainForbiddenCards()
        {
            PlayerService ps = new PlayerService();
            Player player = ps.CreatePlayer("asda", false, true);
            List<object> cards = new List<object>
            {
               new Card("trumpers", 3) as object,
                new Card("trumpers", 21) as object,
                new Card("clubs", 14) as object,
                new Card("diamonds", 14) as object
            };

            player.Hand = cards;
            Assert.Throws<NotSupportedException>(() => ps.MakeAside(player, 4));
        }

        [Test]
        public void FivePlayersAttackerCanCallHimself()
        {
            List<Player> players = Musketeers(5);
            
            DealingService dealingService = new DealingService(true, players);
            PlayerService playerService = new PlayerService();
            
            var attacker = players.First();
            attacker.Attacker = true;
            attacker.Hand.AddRange(Kings());
            playerService.MakeDealer(attacker);            

            var calledPlayer = dealingService.AttackerCallsKing(attacker);

            Assert.That(calledPlayer.Name, Is.EqualTo(attacker.Name));
        }

        [Test]
        public void FivePlayersOnlyAttackerCanCallKing()
        {
            List<Player> players = Musketeers(5);            
            DealingService dealingService = new DealingService(true, players);

            var defender = players.First();
            defender.Attacker = false;
            defender.Dealer = true;

            Assert.Throws<ArgumentException>(() => dealingService.AttackerCallsKing(defender));
        }

        [Test]
        public void FivePlayersCalledKingInAsideOneAttacker()
        {
            List<Player> players = Musketeers(5);            
            PlayerService playerService = new PlayerService();
            DealingService dealingService = new DealingService(true, players);
            DealingRules rules = new DealingRules();
            //var dealingServiceMock = new Mock<IDealingService>().Setup(x => x.SendCardsToAside(Kings()));
            
            players.First().Dealer = true;
            players.First().Attacker = true;

            Player calledPlayer = dealingService.AttackerCallsKing(players.First());

            Assert.That(players.Count(x => x.Attacker is false), Is.EqualTo(4));
            Assert.That(players.First(), Is.SameAs(calledPlayer));
        }
    }
}

