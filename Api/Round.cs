using System;
using System.Collections.Generic;
using System.Text;
using TarotFr.Infrastructure;

namespace TarotFr.Api
{
    public class Round
    {
        private LinkedList<Player> _players = new LinkedList<Player>();
        private readonly bool _roundStartsFromTheLeft = true;
        private readonly int _roundNumber = 0;        
        public int GetNbPlayers() => _players.Count;

        public Round(bool startsFromLeft, LinkedList<Player> players)
        {
            _roundStartsFromTheLeft = startsFromLeft;

            if (players is null) throw new ArgumentOutOfRangeException("Players cannot be null in Round");
            _players = players;
        }

        public Player FindDealer()
        {
            foreach(Player player in _players)
            {
                if (player.IsDealer()) return player;
            }

            return null;
        }

        public bool IsPlayerIn(Player player)
        {
            foreach(Player roundPlayer in _players)
            {
                if (roundPlayer.Name.Equals(player.Name)) return true;
            }

            return false;            
        }

        public Player NextPlayer(Player player)
        {
            return _roundStartsFromTheLeft ? _players.Find(player).NextOrFirst().Value : _players.Find(player).PreviousOrLast().Value;
        }
    }
}
