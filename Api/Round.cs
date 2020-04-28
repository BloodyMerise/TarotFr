﻿using System;
using System.Collections.Generic;
using System.Text;
using TarotFr.Infrastructure;

namespace TarotFr.Api
{
    class Round
    {
        private LinkedList<Player> _players = new LinkedList<Player>();
        private bool _roundStartsFromTheLeft = true;
        private int _roundNumber = 0;        
        public int GetNbPlayers() => _players.Count;

        public Round(bool startsFromLeft, LinkedList<Player> players)
        {
            _roundStartsFromTheLeft = startsFromLeft;
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

        public Player NextPlayer(Player player)
        {
            return _roundStartsFromTheLeft ? _players.Find(player).NextOrFirst().Value : _players.Find(player).PreviousOrLast().Value;
        }
    }
}