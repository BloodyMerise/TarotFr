using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Domain
{
    public class Player
    {
        private string _name;
        private List<Card> _hand;
        private bool _isAttacker;
        private string contract;
        private int _roundScore;
        private int _totalScore;

        public Player(string name)
        {
            _name = name;
        }

        public void TakesCards(TarotDeck dek, int nbcards)
        {
            _hand.AddRange(dek.SelectCards(nbcards));
        }

        public int NbCards() => _hand.Count;
    }
}
