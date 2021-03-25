using System.Collections.Generic;
using TarotFr.Domain;
using TarotFr.Infrastructure;

namespace TarotFr.Api
{
    public class TarotTable
    {        
        private List<Card> _aside { get; }
        private Round _round;               
        
        public TarotTable(bool shuffleCards, bool startsFromLeft,List<Player> players)
        {     
            _aside = new List<Card>();
            _round = new Round(startsFromLeft, players);
        }

        public int CountAside() => _aside.Count;
        public int NbPlayers() => _round.GetNbPlayers();
        public int GetRoundNumber() => _round.RoundNumber;
        public void ResetRoundNumber() => _round.ResetRoundNumber();
                
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

        public void SendCardsToAside(IEnumerable<Card> cards)
        {
            if (!(cards is null)) _aside.AddRange(cards);
        }

        public void SendAsideToPlayerHand(Player player)
        {
            player.Hand.AddRange(_aside);
            _aside.Clear();
        }
    }
}
