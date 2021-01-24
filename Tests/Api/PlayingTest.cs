using TarotFr.Api;
using TarotFr.Domain;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TarotFrTests.Api
{
    class PlayingTest
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
            players.First().Dealer = true;

            return players as List<Player>;
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void TestDistribAndBets(int nbPlayers)
        {
            List<Player> players = Musketeers(nbPlayers);
            TarotTable tb = new TarotTable(true, true, players);
            DealingRules dr = new DealingRules();
            DealingService ds = new DealingService(tb);
            BettingService bs = new BettingService(tb);
            PlayerService ps = new PlayerService();

            ds.DealsAllCardsFromDeck();
            
            Assert.That(tb.NbPlayers(), Is.EqualTo(nbPlayers));
            Assert.That(tb.CountDog(), Is.EqualTo(dr.DogMaxCards(nbPlayers)));
            Assert.NotNull(tb.GetDealer());
            Assert.Zero(tb.GetRoundNumber());
            
            foreach (Player player in players)
            {
                Assert.That(player.Hand.Count == players.First().Hand.Count);
                Assert.NotNull(player.Contract);
            }

            bs.GatherBets(tb);
            bs.SetBetWinnerAsAttacker();
            tb.ResetRoundNumber();            

            Assert.Zero(tb.GetRoundNumber());
            Assert.That( players.Where(x => x.Attacker is true).Count() == 1); // only  1 attacker is known at this stage
        }
    }
}
