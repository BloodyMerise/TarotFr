using TarotFr.Api;
using TarotFr.Domain;
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
        public void TestDistribAndBets(int nbPlayers)
        {                        
            PlayerService ps = new PlayerService();                       
            List<Player> players = ps.CreatePlayers(Musketeers().Take(nbPlayers)).ToList();
            
            DealingRules dr = new DealingRules();
            DealingService ds = new DealingService(false, players);
            BettingService bs = new BettingService();

            players.First().Dealer = true;
            ds.DealsAllCardsFromDeck();
            
            Assert.That(ds.NbPlayers(), Is.EqualTo(nbPlayers));
            Assert.That(ds.CountAside(), Is.EqualTo(dr.AsideMaxCards(nbPlayers)));
            Assert.NotNull(ds.GetDealer());
            Assert.Zero(ds.GetRoundNumber());
                        
            foreach (Player player in players)
            {
                Assert.That(player.Hand.Count == players.First().Hand.Count);
                Assert.That(player.Contract.ToString(), Is.EqualTo("pass"));
            }

            bs.GatherBets(players);
            bs.AuctionIsWon(ds);
            Player attacker = players.FirstOrDefault(x => x.Attacker is true);

            Assert.That( players.Count(x => x.Attacker is true) == 1); // only  1 attacker is known at this stage
                                
            ps.MakeAside(attacker,dr.AsideMaxCards(nbPlayers));

            Assert.That(attacker.Hand.Count, Is.EqualTo(players.FirstOrDefault(x => x.Attacker is false).Hand.Count));            
            Assert.That(attacker.WonHands.Count, Is.EqualTo(dr.AsideMaxCards(nbPlayers)));
        }
    }
}
