using System;
using System.Collections.Generic;
using TarotFr.Infrastructure;
using TarotFr.Domain;
using System.Linq;

namespace TarotFr.Api
{
    public class TarotDeck
    {
        private Stack<Card> _deck;        
       
        public bool IsEmpty() => _deck.Count == 0;
        public Card Pop() => _deck.Pop();
        public int NbCardsInDeck() => _deck.Count;

        public TarotDeck(bool shuffleCards)
        {
            _deck = new Stack<Card>();
            
            FillDeck();
            if (shuffleCards) Shuffle();            
        }

        private bool CheckScore(double expectedScore) { return _deck.Score() == expectedScore; }

        private void FillDeck()
        {
            //Push all Basic cards
            for (int i = 1; i < 15; i++)
            {
                foreach (Colors.CardColors color in Enum.GetValues(typeof(Colors.CardColors)))
                {
                    _deck.Push(new Card(color.ToString(), i));
                }
            }

            //Push rest of Trumpers
            for (int i = 15; i < 22; i++)
            {
                _deck.Push(new Card(Colors.CardColors.trumpers.ToString(), i));
            }

            //Push Excuse
            _deck.Push(new Card(Colors.CardColors.trumpers.ToString(), 0));

            //Check consistency
            if (_deck.Count != DealingRules.MaxCardsInDeck)
            {
                throw new Exception($"Wrong total number of cards: expected {DealingRules.MaxCardsInDeck} but was {_deck.Count}");
            }

            if (!CheckScore(CardCountingRules.MaxScore))
            {
                throw new Exception($"Wrong total score: expected {CardCountingRules.MaxScore} but was {_deck.Score()}");
            }
        }

        //if asked to pop 3 but only 2 in deck, still yields the last 2
        public IEnumerable<Card> Pop(int nbCards)
        {
            if (nbCards < 0) yield break;
            for (int i = 0; i < nbCards; i++)
            {
                if (_deck.Count == 0) yield break;
                yield return _deck.Pop();
            }
        }

        // from https://stackoverflow.com/questions/33643104/shuffling-a-stackt
        public Stack<Card> Shuffle()
        {
            var temp = _deck.ToArray();
            Random rnd = new Random();

            _deck.Clear();
            foreach (Card value in temp.OrderBy(x => rnd.Next()))
            {
                _deck.Push(value);
            }

            return _deck;
        }
    }
}
