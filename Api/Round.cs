using System;
using System.Collections.Generic;
using TarotFr.Infrastructure;

namespace TarotFr.Api
{
    public class Round
    {
        private List<Player> _players;
        private Player _currentPlayer;
        private readonly bool _rotateLeft;
        private readonly int _roundNumber;        
        
        public Round(bool startsFromLeft, List<Player> players)
        {
            _rotateLeft = startsFromLeft;
            _players = new List<Player>();
            _roundNumber = 0;
            _currentPlayer = null;

            if (players is null) throw new ArgumentOutOfRangeException("Players cannot be null for new round");
            _players = players;            
        }

        public int GetNbPlayers() => _players.Count;

        public Player FindDealer()
        {
            foreach(Player player in _players)
            {
                if (player.IsDealer()) return player;
            }

            throw new NullReferenceException($"Couldn't find dealer for this round");
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
            int indexPlayer = _players.IndexOf(player);
            return _rotateLeft ? NextPlayerLeft(indexPlayer) : NextPlayerRight(indexPlayer);
        }

        private void SetCurrentPlayer(Player player)
        {
            _currentPlayer = player;
        }

        public Player NextPlayer()
        {
            if (_currentPlayer is null) SetCurrentPlayer(player: FindDealer());
            int indexCurrentPlayer = _players.IndexOf(_currentPlayer);
            
            return _rotateLeft ? NextPlayerLeft(indexCurrentPlayer) : NextPlayerRight(indexCurrentPlayer);
        }

        private Player NextPlayerRight(int playerIndex)
        {
            if (playerIndex - 1 < 0)
            {
                SetCurrentPlayer(_players[_players.Count - 1]);
                return _players[_players.Count - 1];
            }
            else
            {
                SetCurrentPlayer(_players[playerIndex - 1]);
                return _players[playerIndex  - 1];
            }
        }

        private Player NextPlayerLeft(int playerIndex)
        {
            if (playerIndex + 1 >= _players.Count)
            {
                SetCurrentPlayer(_players[0]);
                return _players[0];
            }
            else
            {
                SetCurrentPlayer(_players[playerIndex + 1]);
                return _players[playerIndex + 1];
            }
        }
    }
}
