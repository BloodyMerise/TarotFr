using System;
using System.Collections.Generic;
using System.Linq;
using TarotFr.Domain;

namespace TarotFr.Api
{
    public class BettingService
    {
        private Dictionary<Player, Contract> _bets;
        public Dictionary<Player, Contract> RegisteredBets() => _bets;

        public BettingService(TarotTable table)
        {
            _bets = new Dictionary<Player, Contract>();
        }

        private void RegisterBets(Player player, Contract contract)
        {
            _bets.Add(player, contract);
        }

        public void GatherBets(TarotTable table)
        {
            while (table.GetRoundNumber() == 0)
            {
                Player nextPlayer = table.NextPlayer();
                RegisterBets(nextPlayer, nextPlayer.Contract.PickRandomly());
            }
            return;
        }

        public List<Contract> AvailableBets()
        {
            List<Contract> availableContracts = new List<Contract>() { new Contract("pass") };
            List<Contract> allContracts = new List<Contract>();

            foreach(var contract in Enum.GetValues(typeof(Contract.Contracts)))
            {
                allContracts.Add(new Contract(contract.ToString()));
            }

            var highestBet = GetWinningBet();
            availableContracts.AddRange(allContracts.Where(x => x > highestBet.Value));

            return availableContracts;
        }

        public KeyValuePair<Player, Contract> GetWinningBet()
        {
            if(_bets is null || _bets.Count == 0)
            {
                return new KeyValuePair<Player, Contract>(null, new Contract("pass"));
            }

            Contract winningBet = _bets.Values.Max();
            Player betWinner = _bets.FirstOrDefault(x => x.Value == winningBet).Key;
            return new KeyValuePair<Player, Contract>(betWinner, winningBet);
        }
    }
}
