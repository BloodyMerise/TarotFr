using System;
using System.Collections.Generic;
using TarotFr.Domain;
using TarotFr.Infrastructure;

namespace TarotFr.Api
{
    public class DealingService : IDealingService
    {
        DealingRules _rules;
        TarotTable _table;
        TarotDeck _deck;
        PlayerService _playerService = new PlayerService();
        int _nbPlayers;

        public bool CheckAside() => _table.CountAside() == _rules.AsideMaxCards(_nbPlayers);
        bool IDealingService.NoMoreCardsInDeckAfterDealing() => _deck.IsEmpty() && _playerService.CountCardsInHand(_table.NextPlayer()) > 0;

        public DealingService(TarotTable table)
        {
            _rules = new DealingRules();
            _deck = new TarotDeck(true);
            _table = table;
            _nbPlayers = _table.NbPlayers();
        }
        
        public void DealsAllCardsFromDeck()
        {
            Player dealer = _table.GetDealer();            
                        
            while (!_deck.IsEmpty())
            {
                _playerService.DealsCards(_deck.Pop(DealingRules.NbCardsToDeal), _table.NextPlayer());
                _table.SendCardsToAside(PickCardsForAside(_deck, _table.CountAside()));                
            }

            _table.ResetRoundNumber();
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
                _table.GetDealer();
            }
            catch (NullReferenceException noDealer)
            {                
                return false;
            }
            return true;
        }
    }
}
