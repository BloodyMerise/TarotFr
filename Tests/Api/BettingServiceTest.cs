using TarotFr.Api;
using TarotFr.Infrastructure;
using TarotFr.Domain;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System;

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

            foreach (var contract in Enum.GetValues(typeof(Contract.Contracts)))
            {
                allContracts.Add(new Contract(contract.ToString()));
            }
            return allContracts;
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void RegisteredBetsForAllPlayers(int nbPlayers)
        {
            List<Player> players = Musketeers(nbPlayers);
            TarotTable tb = new TarotTable(false, false, players);
            BettingService bs = new BettingService(tb);
            
            bs.GatherBets(tb);

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
            List<Player> players = Musketeers(3);
            TarotTable tb = new TarotTable(false, false, players);
            BettingService bs = new BettingService(tb);
            List<Contract> allContracts = AllContracts();
                                
            var availableBets = bs.AvailableBets();
            Assert.That(availableBets.Count == allContracts.Count);

            bs.GatherBets(tb);            
            availableBets = bs.AvailableBets();
            var winningBet = bs.GetWinningBet();
            List<Contract> onlyGreatersPlusPass = allContracts.Where(x => x > winningBet.Value).ToList();

            if (winningBet.Value.ToString() != "pass")
            {
                onlyGreatersPlusPass.Add(new Contract("pass"));
            }

            //fails (differnt HashCode)
            //CollectionAssert.AreEquivalent(onlyGreatersPlusPass,availableBets);

            CollectionAssert.AreEquivalent(onlyGreatersPlusPass.Select(x => x.ToString()),availableBets.Select(x => x.ToString()));
        }               
    }
}
