using System;
using System.Collections.Generic;
using TarotFr.Domain;
using TarotFr.Infrastructure;

namespace TarotFr.Api
{
    public class CardDealingService
    {
        int _limitDog;
        int _step;
       
        public CardDealingService(int nbPlayers)
        {
            _step = CardDealingRules.NbCardsToDeal;
            _limitDog = CardDealingRules.DogMaxCards(nbPlayers);        
        }

        public void DealsAllCardsFromDeck(TarotTable tarotTable)
        {
            TarotDeck deck = new TarotDeck(true);
            Player dealer = tarotTable.GetRoundDealer();
            if (dealer is null) return;  // pick one randomly ?                      

            Player nextPlayer = dealer;

            while (!deck.IsEmpty())
            {
                dealer.DealsCard(deck.Pop(_step), tarotTable.NextPlayer(nextPlayer));
                tarotTable.SendCardsToDog(CardsForDog(deck, tarotTable.CountDogCards()));
                nextPlayer = tarotTable.NextPlayer(nextPlayer);
            }
        }      

        private IEnumerable<Card> CardsForDog(TarotDeck tarotDeck, int dogCardsCount)
        {
            Random rnd = new Random();

            if (dogCardsCount < _limitDog)
            {
                int nbRemainingCards = _limitDog - dogCardsCount;
                return tarotDeck.Pop(Math.Min(rnd.Next(1, _step) + 1, nbRemainingCards));
            }
            else return null;
        }
    }
}
