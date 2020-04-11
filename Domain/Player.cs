using System;
using System.Collections.Generic;

namespace TarotFr.Domain
{
    public class Player
    {
        private string _name;
        private List<Card> _hand = new List<Card>();
        private List<Card> _dog = new List<Card>();
        private LinkedList<List<Card>> _wonHands = new LinkedList<List<Card>>();
        private Contract _contract = new Contract(null);
        private bool _isDealer = false;
        private bool _isAttacker = false;
        private bool _isDefenser = false;

        public Player(string name)
        {
            if (String.IsNullOrEmpty(name)) name = "Josette";
            else _name = name;            
        }

        public int ScoreInHand() { return _hand.Score(); }
        public int ScoreInDog() { return _dog.Score(); }
        public void MakeDealer() { _isDealer = true; }
        public void MakeAttacker() { _isAttacker = true; }
        public void MakeDefenser() { _isDefenser = true; }
        public int NbCardsInHand() => _hand.Count;
        public int NbCardsInDog() => _isDealer ? _dog.Count : 0;
        public bool IsDealer() => _isDealer;

        private void ReceivesCard(Card card)
        {
            _hand.Add(card);
        }

        private void ReceivesCard(IEnumerable<Card> cards)
        {
            _hand.AddRange(cards);
        }

        public void DealsAllCardsFromDeck(TarotTable dek, LinkedList<Player> players)
        {
            if (!_isDealer) return;
            int limitDog = CardDealingRules.DogMaxCards(players.Count);
            int step = CardDealingRules.NbCardsToDeal();

            Random rnd = new Random();

            while (!dek.DeckIsEmpty())
            {
                players.First.NextOrFirst().Value.ReceivesCard(dek.Pop(step));
                if (_dog.Count < limitDog) DealerFromDeckToDog(dek, rnd.Next(1, step+1), limitDog);                                    
            }
        }

        public void DealerFromDeckToDog(TarotTable dek, int nbCards, int limitDog)
        {
            if (_isDealer) {
                for (int i = 0; i < nbCards && _dog.Count < limitDog; i++)
                {
                    if (dek.DeckIsEmpty()) break; 
                    _dog.Add(dek.DeckPop());
                }
            }
        }

        public void GivesHandNbCardsFromDeck(TarotTable dek, int nbcards, Player player)
        {            
            if (_isDealer)
            {
                for (int i = 0; i < nbcards; i++)
                {
                    if (dek.DeckIsEmpty()) break;
                    player._hand.Add(dek.DeckPop());
                }
            }
        }

        public void BetsContract(string contractName)
        {
            if (String.IsNullOrEmpty(contractName)) _contract = new Contract("pass");
            else _contract = new Contract(contractName);
        }
    }
}
