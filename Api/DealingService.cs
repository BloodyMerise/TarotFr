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

        public bool CheckDog() => _table.CountDog() == _rules.DogMaxCards(_nbPlayers);
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
                _table.SendCardsToDog(PickCardsForDog(_deck, _table.CountDog()));                
            }

            _table.ResetRoundNumber();
        }      

        private IEnumerable<Card> PickCardsForDog(TarotDeck tarotDeck, int dogCardsCount)
        {            
            int nbRemainingCardsForDog = _rules.DogMaxCards(_nbPlayers) - dogCardsCount;           

            if (nbRemainingCardsForDog > 0)
            {
                Random rnd = new Random();
                int rndNbCardsToSend = rnd.Next(1, DealingRules.NbCardsToDeal) + 1;

                return tarotDeck.Pop(Math.Min(rndNbCardsToSend, nbRemainingCardsForDog));
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
