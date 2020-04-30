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

        public int NbCardsInDog() => _dog.Count;

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

            Player nextPlayer = _dealer;

            while (!tarotTable.DeckIsEmpty())
            {
                _dealer.DealsCard(tarotTable.Pop(_step), tarotTable.NextPlayer(nextPlayer));
                DealsCardsToDog(tarotTable);
                nextPlayer = tarotTable.NextPlayer(nextPlayer);
            }
        }

        private void DealsCardsToDog(TarotTable tarotTable)
        {
            Random rnd = new Random();
            int nbcards = 0;

            if (_dog.Count < _limitDog)
            {
                int nbRemainingCards = _limitDog - _dog.Count;
                _dog.AddRange(tarotTable.Pop(Math.Min(rnd.Next(1, _step) + 1,nbRemainingCards)));
            }
        }
    }
}
