using System;
using System.Collections.Generic;
using TarotFr.Domain;
using TarotFr.Infrastructure;

namespace TarotFr.Api
{
    public class TarotTable
    {        
        private List<Card> _dog { get; }
        private Round _round;               
        
        public TarotTable(bool shuffleCards, bool startsFromLeft,List<Player> players)
        {     
            _dog = new List<Card>();
            _round = new Round(startsFromLeft, players);
        }

        public int CountDog() => _dog.Count;
        public int NbPlayers() => _round.GetNbPlayers();
        public int GetRoundNumber() => _round.RoundNumber;
        public void ResetRoundNumber() => _round.ResetRoundNumber();
        public void SendCardsToDog(IEnumerable<Card> cards) { if(!(cards is null)) _dog.AddRange(cards); }
        
        public Player NextPlayer(Player player)
        {            
            return _round.NextPlayer(player);
        }

        public Player NextPlayer()
        {
            return _round.NextPlayer();
        }

        public Player GetDealer()
        {
            return _round.FindDealer();
        }
    }
}
