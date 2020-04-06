using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Domain
{
    public class Player
    {
        private string _name;
        private List<Card> _hand = new List<Card>();
        private bool _isAttacker = false;
        private bool _isDealer = false;
        private string _contract = null;
        private int _roundScore = 0;
        private int _totalScore = 0;

        public void MakeDealer() => _isDealer = true;
        public void MakeAttacker() => _isAttacker = true;
        public void MakeNotDealer() => _isDealer = false;
        public void MakeNotAttacker() => _isAttacker = false;
        public bool IsDealer() => _isDealer;
        public bool IsAttacker() => _isAttacker;

        public Player(string name)
        {
            _name = name;            
        }

        public void Score() { } 

        public void DealsCards(TarotDeck dek, int nbcards, Player player)
        {
            if (!_isDealer) return;
            if (nbcards > dek.NbCardsInDeck()) return;
            if (nbcards <= 0) return;
            player.ReceivesCard(dek.Take(nbcards));
        }

        public int NbCards() => _hand.Count;

        public void ReceivesCard(IEnumerable<Card> cards)
        {
            _hand.AddRange(cards);
        }
    }
}
