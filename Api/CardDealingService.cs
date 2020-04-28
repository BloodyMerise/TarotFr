using System;
using System.Collections.Generic;
using System.Text;
using TarotFr.Domain;
using TarotFr.Infrastructure;

namespace TarotFr.Api
{
    public class CardDealingService
    {
        Player _dealer;
        List<Card> _dog;
        int _limitDog;
        int _step;

        public CardDealingService(int nbPlayers)
        {
            _step = CardDealingRules.NbCardsToDeal();
            _limitDog = CardDealingRules.DogMaxCards(nbPlayers);
            _dog = new List<Card>();
        }

        public void DealsAllCardsFromDeck(TarotTable tarotTable)
        {
            _dealer = tarotTable.GetRoundDealer();
            if (_dealer is null) return;  // pick one randomly ?                      

            Random rnd = new Random();
            Player nextPlayer = _dealer;

            while (!tarotTable.DeckIsEmpty())
            {
                _dealer.DealsCard(tarotTable.Pop(_step), tarotTable.NextPlayer(nextPlayer));

                if(_dog.Count < _limitDog)
                {
                    int nbRemainingCards = _limitDog - _dog.Count;
                    _dog.AddRange(tarotTable.Pop(rnd.Next(1, Math.Max(_step, nbRemainingCards) + 1)));
                }

                nextPlayer = tarotTable.NextPlayer(nextPlayer);
            }
        }
    }
}
