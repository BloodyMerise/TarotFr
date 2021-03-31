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

        public BettingService()
        {
            _bets = new Dictionary<Player, Contract>();
        }

        private void RegisterBets(Player player, Contract contract)
        {
            _bets.Add(player, contract);
            player.Contract = contract;
        }

        public void GatherBets(List<Player> players)
        {
            var availableBets = AvailableBets();            
            
            foreach (var player in players)
            {
                Contract bet = _ps.AskForBet(player, availableBets);
                RegisterBets(player, bet);
            }

            return;
        }

        public void AuctionIsWon(DealingService ds)
        {            
            var winningBet = GetWinningBet();
            Player winner = winningBet.Key;

            winner.Attacker = true;
            ds.SendAsideToPlayerHand(winner);
            if (_bets.Count == 5)
            {                
                ds.AttackerCallsKing(winner);
            }

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
