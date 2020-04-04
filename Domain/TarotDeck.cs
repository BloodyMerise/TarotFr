using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace TarotFr.Domain
{
    public class TarotDeck
    {
        private List<Card> _tarotDeck = new List<Card>();

        public TarotDeck()
        {
            List<string> colors = new List<string> { "hearts", "spades", "diamonds", "clubs" };

            for (int i = 1; i < 15; i++)
            {
                foreach (string color in colors)
                {
                    _tarotDeck.Add(new Card(color, i));
                }
            }
            for (int i = 0; i < 22; i++)
            {
                _tarotDeck.Add(new Card("trumpers", i));
            }
        }

        public IEnumerable<Card> SelectCards(string color = null, int? points = null)
        {
            if (!points.HasValue && !String.IsNullOrEmpty(color))
            {
                return _tarotDeck.Where(card => card.Color() == color);
            }
            else if (points.HasValue && !String.IsNullOrEmpty(color))
            {
                return _tarotDeck.Where(card => card.Color() == color && card.Points() == points);
            }
            else if (points.HasValue && String.IsNullOrEmpty(color))
            {
                return _tarotDeck.Where(card => card.Points() == points);
            }
            else
            {
                return _tarotDeck;
            }
        }

        public IEnumerable<Card> SelectCards(List<Card> cards)
        {            
            return cards.Intersect(_tarotDeck);
        }

        public IEnumerable<Card> SelectCards(int nbCards)
        {
            if (_tarotDeck.Count >= nbCards) return _tarotDeck.Take<Card>(nbCards);
            else return null;
        }

        public Card Pick(int index = -1)
        {
            return _tarotDeck.ElementAt(index);
        }

        public List<Card> Shuffle()
        {
             _tarotDeck.Shuffle();
            return _tarotDeck;
        }
    }

    //The below is from https://stackoverflow.com/questions/273313/randomize-a-listt
    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static Random Local;

        public static Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }
    }
    static class MyExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
