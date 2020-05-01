using System;
using System.Collections.Generic;
using TarotFr.Infrastructure;

namespace TarotFr.Api
{
    public class TarotTable
    {        
        private List<Card> _dog { get; }
        private Round _round;               
        
        public TarotTable(bool shuffleCards, bool startsFromLeft,LinkedList<Player> players)
        {     
            _dog = new List<Card>();
            _round = new Round(startsFromLeft, players);
        }

        public int CountDogCards() => _dog.Count;
        public void SendCardsToDog(IEnumerable<Card> cards) { if(!(cards is null)) _dog.AddRange(cards); }
        
        public Player NextPlayer(Player player)
        {
            // if player not in round, crash ? null ?
            return _round.NextPlayer(player);
        }
        
        public Player AuctionWinner()
        {
            return new Player(null);
        }

        public Player GetRoundDealer()
        {
            return _round.FindDealer();
        }
    }
}
