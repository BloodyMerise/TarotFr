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
        private PlayerService _ps = new PlayerService();

        public BettingService(TarotTable table)
        {
            _bets = new Dictionary<Player, Contract>();
        }

        private void RegisterBets(Player player, Contract contract)
        {
            _bets.Add(player, contract);
            player.Contract = contract;
        }

        public void GatherBets(TarotTable table)
        {
            var availableBets = AvailableBets();
            
            while (table.GetRoundNumber() == 0)
            {
                Player nextPlayer = table.NextPlayer();
                Contract bet = _ps.AskForBet(nextPlayer, availableBets);
                RegisterBets(nextPlayer, bet);
            }

            table.ResetRoundNumber();

            return;
        }

        public void SetBetWinnerAsAttacker()
        {
            var winningBet = GetWinningBet();
            winningBet.Key.Attacker = true;

            return;
        }

        public List<Contract> AvailableBets()
        {
            List<Contract> availableContracts = new List<Contract>() { new Contract("pass") };
            List<Contract> allContracts = ListAllContracts();

            Contract highestContractBet = _bets.Count == 0 ? new Contract("pass") : _bets.Values.Max();
            availableContracts.AddRange(allContracts.Where(x => x > highestContractBet));

            return availableContracts;
        }

        private List<Contract> ListAllContracts()
        {
            List<Contract> allContracts = new List<Contract>();
            foreach (string contract in new Contract(null).GetAll())
            {
                allContracts.Add(new Contract(contract));
            }

            return allContracts;
        }

        public KeyValuePair<Player, Contract> GetWinningBet()
        {
            if (_bets is null || _bets.Count == 0)
            {
                throw new ArgumentNullException("bet", "No bet has been registered yet");
            }

            Contract winningBet = _bets.Values.Max();
            Player betWinner = _bets.FirstOrDefault(x => x.Value == winningBet).Key;
            return new KeyValuePair<Player, Contract>(betWinner, winningBet);
        }
    }
}
