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
                IEnumerable<Card> possibleCardsThisRound = GetPlayableCardsForPlayer(_currentPlayer, cardsPlayedThisRound.Select(x => x.Item2));
                playerPlay = playerService.AskPlayerCard(_currentPlayer, possibleCardsThisRound);
                cardsPlayedThisRound.Add(new Tuple<Player, Card>(_currentPlayer, playerPlay));
                _currentPlayer.Hand.Remove(playerPlay);
                NextPlayer();
            }

            Player roundWinner = RoundWinner(cardsPlayedThisRound);
            roundWinner.WonHands.AddRange(cardsPlayedThisRound.Select(x => x.Item2));

            _cardsPlayed.Push(cardsPlayedThisRound);
                       
            return cardsPlayedThisRound;
        }

        public IEnumerable<Card> GetPlayableCardsForPlayer(Player player,IEnumerable<Card> cardsPlayed)
        {
            List<Card> playerHand = player.Hand.Cast<Card>().ToList();
            Card firstCard = cardsPlayed.Count() == 0 ? new Card("trumpers", 0) : cardsPlayed.First();
            
            //If excuse is played first, can play any card
            if(firstCard.IsOudler() && firstCard.Points() == 0)
            {
                return playerHand;
            }

            //Can always play Excuse if player has it
            //TODO: excuse is lost if played on last hand
            List<Card> possibleCards = player.Hand.Cast<Card>().Where(x => x.IsOudler() && x.Points() == 0).ToList();

            //Must play asked color if player has any
            if (playerHand.Select(x => x.Color).Contains(firstCard.Color))
            {
                if (firstCard.IsTrumper())
                {
                    possibleCards.AddRange(GetPossibleTrumpersOrDefault(playerHand, cardsPlayed));
                }
                else
                {
                    possibleCards.AddRange(playerHand.Where(x => x.Color == firstCard.Color));
                }                
            }
            //If player doesn't have the first color, check if player can play trumper
            else if (playerHand.Count(x => x.IsTrumper()) > 0)
            {
               possibleCards.AddRange(GetPossibleTrumpersOrDefault(playerHand, cardsPlayed));
            }
            //If player doesn't have same color nor trumper, player can play any card
            else return playerHand;

            return possibleCards;
        }

        public IEnumerable<Card> GetPossibleTrumpersOrDefault(List<Card> playerHand, IEnumerable<Card> cardsPlayed)
        {            
            var highestTrumper = cardsPlayed.Where(x => x.IsTrumper()).Max();
            playerHand.Remove(new Card("trumpers", 0)); // remove Excuse from possibilities -> handled before

            // if no trumper is played, can play any trumper
            if (highestTrumper is null && playerHand.Count(x => x.IsTrumper()) > 0)
            {
                return playerHand.Where(x => x.IsTrumper());
            }
            // if one trumper has been played, must play a higher one if any
            else if (playerHand.Count(x => x.IsTrumper() && x > highestTrumper) > 0)
            {
                return playerHand.Where(x => x.IsTrumper() && x > highestTrumper);
            }
            // if one trumper has been played, but doesn't have higher, must play lower trumper if any
            else if (playerHand.Count(x => x.IsTrumper() && x < highestTrumper) > 0)
            {
                return playerHand.Where(x => x.IsTrumper());
            }
            // If player doesn't have any trumper, but should play one, he must play any other card
            else
            {
                return playerHand;
            }
        }            
    }
}

