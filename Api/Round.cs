using System;
using System.Collections.Generic;
using System.Linq;
using TarotFr.Domain;

namespace TarotFr.Api
{
    public class Round
    {
        public List<Player> players;
        private Player _currentPlayer;
        private readonly bool _rotateLeft;
        private int _nbTimesPlayed;
        public int RoundNumber => _nbTimesPlayed / players.Count;

        public int GetNbPlayers() => players.Count;
        public Player FindDealer() => players.Where(x => x.Dealer == true).First();
        public bool IsPlayerIn(Player player) => players.Any(x => x.Name.Equals(player.Name));


        public Round(bool startsFromLeft, List<Player> players)
        {
            _rotateLeft = startsFromLeft;
            this.players = new List<Player>();
            _nbTimesPlayed = 0;
            _currentPlayer = null;

            if (players is null) throw new ArgumentOutOfRangeException("Players cannot be null for new round");
            this.players = players;            
        }

        internal void ResetRoundNumber()
        {
            _nbTimesPlayed = 0;
        }

        public Player NextPlayer(Player player)
        {   
            if(player is null)
            {
                if (_currentPlayer is null) SetCurrentPlayer(player: FindDealer());
            }

            int indexPlayer = players.IndexOf(_currentPlayer);
            _nbTimesPlayed++;

            return _rotateLeft ? NextOrFirstLeft(indexPlayer) : NextOrFirstRight(indexPlayer);
        }

        private void SetCurrentPlayer(Player player)
        {
            _currentPlayer = player;
        }

        public Player NextPlayer()
        {
            return NextPlayer(null);
        }

        private Player NextOrFirstRight(int playerIndex)
        {
            if (playerIndex - 1 < 0)
            {
                SetCurrentPlayer(players.Last());
                return players.Last();
            }
            else
            {
                SetCurrentPlayer(players[playerIndex - 1]);
                return players[playerIndex  - 1];
            }
        }

        private Player NextOrFirstLeft(int playerIndex)
        {
            if (playerIndex + 1 >= players.Count)
            {
                SetCurrentPlayer(players.First());
                return players.First();
            }
            else
            {
                SetCurrentPlayer(players[playerIndex + 1]);
                return players[playerIndex + 1];
            }
        }
    }
}
