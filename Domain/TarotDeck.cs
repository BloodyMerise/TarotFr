using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace TarotFr.Domain
{
    public class TarotDeck
    {
        public Stack<Card> _tarotDeck { get; }
        private List<Card> _dog { get; }
        private List<Player> _players = new List<Player>();

        public TarotDeck(bool fill,bool shuffle)
        {
            _tarotDeck = new Stack<Card>();
            _dog = new List<Card>();
            if (fill) FillDeck();
            if (shuffle) Shuffle();
        }

        private void FillDeck()
        {
            List<string> colors = new List<string> { "hearts", "spades", "diamonds", "clubs" };

            for (int i = 1; i < 15; i++)
            {
                foreach (string color in colors)
                {
                    _tarotDeck.Push(new Card(color, i));
                }
            }
            for (int i = 0; i < 22; i++)
            {
                _tarotDeck.Push(new Card("trumpers", i));
            }
        }

        public IEnumerable<Card> Take(int nbCards)
        {
            if (nbCards > _tarotDeck.Count) throw new ArgumentOutOfRangeException("nbCards", nbCards, $"cannot take {nbCards} from deck {_tarotDeck.Count}");
            if (nbCards < 0) throw new ArgumentOutOfRangeException("nbCards", nbCards, $"cannot take {nbCards} from deck {_tarotDeck.Count}");

            return _tarotDeck.Take(nbCards);
        }

        public IEnumerable<Card> TakeAll()
        {         
            return _tarotDeck.Take(_tarotDeck.Count);
        }

        public Card Pop()
        {
            return _tarotDeck.Pop();
        }


        // from https://stackoverflow.com/questions/33643104/shuffling-a-stackt
        public Stack<Card> Shuffle()
        {
            var temp = _tarotDeck.ToArray();
            Random rnd = new Random();

            _tarotDeck.Clear();
            foreach (Card value in temp.OrderBy(x => rnd.Next()))
            {
                _tarotDeck.Push(value);
            }

            return _tarotDeck;
        }

        public int NbCardsInDeck()
        {
            return _tarotDeck.Count;
        }
    }
}
