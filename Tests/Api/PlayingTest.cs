using TarotFr.Api;
using TarotFr.Domain;
using TarotFr.Infrastructure;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TarotFrTests.Api
{
    class PlayingTest
    {
        private List<string> Musketeers()
        {
            return new List<string>
            {
                "D'Artagnan",
                "Atos",
                "Portos",
                "Aramis",
                "Albert"
            };           
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void PlayOneGame(int nbPlayers)
        {
            DealingRules dealingRules = new DealingRules();
            BettingService bettingService = new BettingService();
            PlayerService playerService = new PlayerService();                       
            List<Player> players = playerService.CreatePlayers(Musketeers().Take(nbPlayers)).ToList();                       
            DealingService dealingService = new DealingService(false, players);
            RoundService roundService = new RoundService(false, players);
            
            Player dealer = players.First();
            dealer.Dealer = true;
            dealingService.DealsAllCardsFromDeck();
            
            Assert.That(dealingService.NbPlayers(), Is.EqualTo(nbPlayers));
            Assert.That(dealingService.CountAside(), Is.EqualTo(dealingRules.AsideMaxCards(nbPlayers)));
            Assert.NotNull(dealingService.GetDealer());
            Assert.Zero(dealingService.GetRoundNumber());
            Assert.That(dealingService.DealtAsideIsCorrect(), Is.True);

            foreach (Player player in players)
            {
                Assert.That(player.Hand.Count == players.First().Hand.Count);
                Assert.That(player.Contract.ToString(), Is.EqualTo("pass"));
            }

            bettingService.GatherBets(players);
            bettingService.AuctionIsWon(dealingService);
            Player attacker = players.FirstOrDefault(x => x.Attacker is true);

            Assert.That(players.Count(x => x.Attacker is true) == 1); // only  1 attacker is known at this stage
                                            
            playerService.MakeAside(attacker, dealingService.NbCardsInAside());

            // Put this in dealing service ?
            // Game is ready to start when:
            Assert.That(attacker.Hand.Count, Is.EqualTo(players.FirstOrDefault(x => x.Attacker is false).Hand.Count));            
            Assert.That(attacker.WonHands.Count, Is.EqualTo(dealingRules.AsideMaxCards(nbPlayers)));
            Assert.Zero(dealingService.GetRoundNumber());
            
            while (dealer.Hand.Count > 0)
            {
                roundService.PlayRound();
            }

            Assert.Zero(players.Select(x => x.Hand.Count).Sum());
            Assert.That(players.Select(x => x.WonHands.Cast<Card>().Score()).Sum(), Is.EqualTo(CardCountingRules.MaxScore));
        }
    }
}
