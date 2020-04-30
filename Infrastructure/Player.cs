using System;
using System.Collections.Generic;
using TarotFr.Api;

namespace TarotFr.Infrastructure
{
    public class Player
    {
        public string name { get; }
        private List<Card> _hand { get; set; }
        private List<Card> _dog { get; set;}
        private LinkedList<List<Card>> _wonHands = new LinkedList<List<Card>>();
        private Contract _contract = new Contract(null);
        private bool _isDealer = false;
        private bool _isAttacker = false;
        private bool _isDefenser = false;

        public Player(string name)
        {
            if (String.IsNullOrEmpty(name)) name = "LeGrandMamamouchi";
            else this.name = name;
            _hand = new List<Card>();
            _dog = new List<Card>();
        }

        public void MakeDealer() { _isDealer = true; }
        public void MakeAttacker() { _isAttacker = true; }
        public void MakeDefenser() { _isDefenser = true; }
        public int NbCardsInHand() => _hand.Count;
        public double ScoreInHand() => _hand.Score();
        public int NbCardsInDog() => _dog.Count;
        public double ScoreInDog() => _dog.Score();
        public bool IsDealer() => _isDealer;

        private void ReceivesCard(Card card)
        {
            _hand.Add(card);
        }

        private void ReceivesCard(IEnumerable<Card> cards)
        {
            _hand.AddRange(cards);
        }   
        
        public void DealsCard(Card card, Player player)
        {
            if (_isDealer) player.ReceivesCard(card);
        }

        public void DealsCard(IEnumerable<Card> cards, Player player)
        {
            if (_isDealer) player.ReceivesCard(cards);
        }

        public void BetsContract(string contractName)
        {
            if (String.IsNullOrEmpty(contractName)) _contract = new Contract("pass");
            else _contract = new Contract(contractName);
        }
    }
}
