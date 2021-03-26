using System;
using System.Collections.Generic;
using System.Linq;
using TarotFr.Domain;
using TarotFr.Infrastructure;

namespace TarotFr.Api
{
    public class DealingService : IDealingService
    {
        DealingRules _rules;
        TarotTable _tb;
        TarotDeck _deck;
        PlayerService _ps = new PlayerService();
        int _nbPlayers;

        public bool CheckAside() => _tb.CountAside() == _rules.AsideMaxCards(_nbPlayers);
        bool IDealingService.NoMoreCardsInDeckAfterDealing() => _deck.IsEmpty() && _ps.CountCardsInHand(_tb.NextPlayer()) > 0;

        public DealingService(TarotTable table)
        {
            _rules = new DealingRules();
            _deck = new TarotDeck(true);
            _tb = table;
            _nbPlayers = _tb.NbPlayers();
        }
        
        public void DealsAllCardsFromDeck()
        {
            Player dealer = _tb.GetDealer();            
                        
            while (!_deck.IsEmpty())
            {
                _ps.DealsCards(_deck.Pop(DealingRules.NbCardsToDeal), _tb.NextPlayer());
                _tb.SendCardsToAside(PickCardsForAside(_deck, _tb.CountAside()));                
            }

            _tb.ResetRoundNumber();
        }      

        private IEnumerable<Card> PickCardsForAside(TarotDeck tarotDeck, int asideCardsCount)
        {            
            int nbRemainingCardsForAside = _rules.AsideMaxCards(_nbPlayers) - asideCardsCount;           

            if (nbRemainingCardsForAside > 0)
            {
                Random rnd = new Random();
                int rndNbCardsToSend = rnd.Next(1, DealingRules.NbCardsToDeal) + 1;

                return tarotDeck.Pop(Math.Min(rndNbCardsToSend, nbRemainingCardsForAside));
            }
            else return null;
        }

        bool IDealingService.CheckPlayersHaveSameNbCardsInHand()
        {
            throw new NotImplementedException();
        }

        public bool TableHasDealer()
        {
            try
            {
                _tb.GetDealer();
            }
            catch (NullReferenceException noDealer)
            {                
                return false;
            }
            return true;
        }

        public Player AttackerCallsKing(Player player)
        {
            if (player.Attacker is false)
            {
                throw new ArgumentException("Only an attacker can call a king");
            }

            var kings = new List<Card> {
                new Card("hearts",14),
                new Card("diamonds",14),
                new Card("clubs",14),
                new Card("spades",14)
            };

            var calledKing = _ps.AskPlayerCard(player, kings, 1).FirstOrDefault();

            while(_tb.GetRoundNumber() == 0)
            {
                var nextPlayer = _tb.NextPlayer();
                if (nextPlayer.Hand.Contains(calledKing as object))
                {
                    _tb.ResetRoundNumber();
                    return nextPlayer;
                }
            }
            _tb.ResetRoundNumber();

            //King is in aside (maybe more defensive programming here?)
            return player;            
        }
    }
}
