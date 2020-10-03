using System;
using System.Collections.Generic;
using TarotFr.Domain;
using TarotFr.Infrastructure;

namespace TarotFr.Api
{
    class BettingService
    {
        public Dictionary<User, Contract> _bets;

        public BettingService()
        {
            _bets = new Dictionary<User, Contract>();
        }

        private void RegisterBets(User user, Contract contract)
        {
            _bets.Add(user, contract);
        }

        public void GatherBets(TarotTable table)
        {
            return;
        }
    }
}
