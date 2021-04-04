using System;
using System.Collections.Generic;
using System.Linq;
using TarotFr.Domain;
using TarotFr.Infrastructure;

namespace TarotFr.Api
{
    public class RoundService
    {
        public List<Player> Players;
        private Player _currentPlayer;        
        private readonly bool _rotateLeft;
        private int _nbTimesPlayed;
        public int RoundNumber => _nbTimesPlayed / Players.Count;

        public int GetNbPlayers() => Players.Count;
        public Player FindDealer() => Players.First(x => x.Dealer == true);
        public bool IsPlayerIn(Player player) => Players.Any(x => x.Name.Equals(player.Name));
        private Stack<List<Tuple<Player, Card>>> _cardsPlayed;

        public RoundService(bool startsFromLeft, List<Player> players)
        {
            _rotateLeft = startsFromLeft;
            Players = new List<Player>();
            _nbTimesPlayed = 0;
            _currentPlayer = null;
            _cardsPlayed = new Stack<List<Tuple<Player, Card>>>();
            
            if (players is null) throw new ArgumentOutOfRangeException("Players cannot be null for new round");
            Players = players;            
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

            int indexPlayer = Players.IndexOf(_currentPlayer);
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
                SetCurrentPlayer(Players.Last());
                return Players.Last();
            }
            else
            {
                SetCurrentPlayer(Players[playerIndex - 1]);
                return Players[playerIndex  - 1];
            }
        }

        private Player NextOrFirstLeft(int playerIndex)
        {
            if (playerIndex + 1 >= Players.Count)
            {
                SetCurrentPlayer(Players.First());
                return Players.First();
            }
            else
            {
                SetCurrentPlayer(Players[playerIndex + 1]);
                return Players[playerIndex + 1];
            }
        }

        public Player PlayerStartingRound()
        {
            if (_cardsPlayed.Count == 0)
            {
                return NextPlayer();
            }
            else return RoundWinner(_cardsPlayed.Peek());
        }

        public Player RoundWinner(List<Tuple<Player, Card>> cardRound)
        {
            if (cardRound is null) throw new ArgumentNullException();
            cardRound.First().Item2.SetIsFirst();
            Card winningCard = cardRound.Max(x => x.Item2);
            return cardRound.First(x => x.Item2.ToString() == winningCard.ToString()).Item1;
        }

        public List<Tuple<Player, Card>> PlayRound()
        {            
            PlayerService playerService = new PlayerService();
            List<Tuple<Player, Card>> cardsPlayedThisRound = new List<Tuple<Player, Card>>();
            SetCurrentPlayer(PlayerStartingRound());
            Card playerPlay;

            while (cardsPlayedThisRound.Count < GetNbPlayers())
            {
                playerPlay = playerService.AskPlayerCard(_currentPlayer, _currentPlayer.Hand.Cast<Card>());
                cardsPlayedThisRound.Add(new Tuple<Player, Card>(_currentPlayer, playerPlay));
                _currentPlayer.Hand.Remove(playerPlay);
                NextPlayer();
            }

            cardsPlayedThisRound.First().Item2.SetIsFirst();
            _cardsPlayed.Push(cardsPlayedThisRound);
            return cardsPlayedThisRound;
        }
    }
}

