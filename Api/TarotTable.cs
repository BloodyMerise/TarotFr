using System;
using System.Collections.Generic;
using System.Linq;
using TarotFr.Infrastructure;

namespace TarotFr.Api
{
    public class TarotTable
    {
        public Stack<Card> _tarotDeck { get; }
        private List<Card> _dog { get; }
        private Round _round;

        public bool DeckIsEmpty() { return _tarotDeck.Count == 0; }
        public bool CheckScore(int expectedScore) { return _tarotDeck.Score() == expectedScore; }
        public Card DeckPop() { return _tarotDeck.Pop(); }
        public IEnumerable<Card> TakeAll() { return _tarotDeck.Take(_tarotDeck.Count); }
        public int NbCardsInDeck() { return _tarotDeck.Count; }
        
        public TarotTable(bool shuffleCards, bool startsFromLeft,LinkedList<Player> players)
        {
            _tarotDeck = new Stack<Card>();
            _dog = new List<Card>();
            _round = new Round(startsFromLeft, players);            

            FillDeck();
            
            if (shuffleCards) Shuffle();
            if(!CheckScore(91)) throw new Exception("Wrong total score for deck at creation");
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
        
        //if asked to pop 3 but only 2 in deck, still yields the last 2
        public IEnumerable<Card> Pop(int nbCards)
        {            
            if (nbCards < 0) yield return null;
            for(int i = 0; i < nbCards; i++)
            {
                if (_tarotDeck.Count == 0) break;
                yield return _tarotDeck.Pop();
            }
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

        public void AddPlayers(LinkedList<Player> players)
        {
            _round = new Round(true, players);
        }
        
        public Player NextPlayer(Player player)
        {
            // if player not in round, crash ? null ?
            return _round.NextPlayer(player);
        }
        
        public Player AuctionWinner()
        {
            return new Player(null);
        }

        public Player GetRoundDealer()
        {
            return _round.FindDealer();
        }
    }
}
