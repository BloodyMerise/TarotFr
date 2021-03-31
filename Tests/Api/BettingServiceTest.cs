using TarotFr.Api;
using TarotFr.Domain;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TarotFrTests.Api
{
    class BettingServiceTest
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

        private List<Contract> AllContracts()
        {
            List<Contract> allContracts = new List<Contract>();

            foreach (var contract in new Contract(null).GetAll())
            {
                allContracts.Add(new Contract(contract.ToString()));
            }
            return allContracts;
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void RegisteredBetsMatchForAllPlayers(int nbPlayers)
        {
            List<Player> players = Musketeers(nbPlayers);            
            BettingService bs = new BettingService();
            
            bs.GatherBets(players);

            Assert.That(bs.RegisteredBets().Count == nbPlayers);

            var registeredBets = bs.RegisteredBets();
            foreach(Player player in players)
            {
                Assert.That(registeredBets[player].ToString() == player.Contract.ToString());
            }
        }
        
        [Test]
        public void ListAvailableContracts()
        {
            List<Player> players = Musketeers(5);        
            BettingService bs = new BettingService();
            List<Contract> allContracts = AllContracts();

            var availableBets = bs.AvailableBets();
            CollectionAssert.AreEquivalent(availableBets.Select(x => x.ToString()), allContracts.Select(x => x.ToString()));

            bs.GatherBets(players);
            availableBets = bs.AvailableBets();
            var winningBet = bs.GetWinningBet();
            var onlyGreatersPlusPass = allContracts.Where(x => x > winningBet.Value).Select(x => x.ToString()).ToList();

            if (winningBet.Value.ToString() != "pass")
            {
                onlyGreatersPlusPass.Add(new Contract("pass").ToString());
            }

            //fails (different HashCode)
            //CollectionAssert.AreEquivalent(onlyGreatersPlusPass,availableBets);

            CollectionAssert.AreEquivalent(onlyGreatersPlusPass,availableBets.Select(x => x.ToString()));
        }

        [Test]
        public void PlayerWithWinningContractIsAttacker()
        {
            List<Player> players = Musketeers(5);
            DealingService ds = new DealingService(true,players);
            BettingService bs = new BettingService();

            bs.GatherBets(players);
            var bet = bs.GetWinningBet();
            bs.AuctionIsWon(ds);

            Assert.True(bet.Key.Attacker);
        }
    }
}
