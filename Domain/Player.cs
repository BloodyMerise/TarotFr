using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Domain
{
    public class Player
    {
        private string _name;
        private List<Card> _hand = new List<Card>();
        private bool _isDealer = false;
        private List<Card> _dog = new List<Card>();

        public Player(string name)
        {
            _name = name;            
        }

        public int ScoreInHand() { return _hand.Score(); }
        public int ScoreInDog() { return _dog.Score(); }
        public void MakeDealer() { _isDealer = true; }
        public int NbCardsInHand() => _hand.Count;
        public int NbCardsInDog() => _isDealer ? _dog.Count : 0;

        private void ReceivesCard(Card card)
        {
            _hand.Add(card);
        }

        private void ReceivesCard(IEnumerable<Card> cards)
        {
            _hand.AddRange(cards);
        }

        public void DealsAllCardsFromDeck(TarotDeck dek, int step, LinkedList<Player> players, int limitDog)
        {
            if (!_isDealer) return;            
            while (!dek.IsEmpty())
            {
                players.First.NextOrFirst().Value.ReceivesCard(dek.Pop(step));
                if (_dog.Count < limitDog) DealerFromDeckToDog(dek, 1, limitDog);                                    
            }
        }

        public void DealerFromDeckToDog(TarotDeck dek, int nbcards, int limitDog)
        {
            if (_isDealer && nbcards < limitDog ) {
                for (int i = 0; i < nbcards; i++)
                {
                    if (dek.IsEmpty()) break; 
                    _dog.Add(dek.Pop());
                }
            }
        }

        public void GivesHandNbCardsFromDeck(TarotDeck dek, int nbcards, Player player)
        {
            if (_isDealer)
            {
                for (int i = 0; i < nbcards; i++)
                {
                    if (dek.IsEmpty()) break;
                    player._hand.Add(dek.Pop());
                }
            }
        }
    }
}
