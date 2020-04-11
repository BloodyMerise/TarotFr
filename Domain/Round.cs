using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Domain
{
    internal class Round
    {
        private LinkedList<Player> _players = new LinkedList<Player>();
        private bool _roundStartsFromTheLeft = true;
        private int _roundNumber = 0;

        public Round(bool startsFromLeft)
        {
            _roundStartsFromTheLeft = startsFromLeft;
        }

        private LinkedListNode<Player> FindDealerNode()
        {
            while (!_players.First.Next.Value.IsDealer()) {}
            return _players.First.Next;
        }

        public void SetRoundToStartAfterDealer()
        {
            LinkedListNode<Player> dealerNode = FindDealerNode();            
        }

        private Player NextPlayer()
        {
            if (_roundStartsFromTheLeft) return _players.First.Next.Value;
            else return _players.First.Previous.Value;
        }
    }
}
