﻿using System;
using System.Collections.Generic;
using TarotFr.Domain;
using TarotFr.Infrastructure;

namespace TarotFr.Api
{
    public class PlayerService : IPlayerService
    {
        private LinkedList<List<Card>> _wonHands;
        
        public void DealsCards(IEnumerable<Card> cards, Player player)
        {
            player.Hand.AddRange(cards);
        }

        public Player CreatePlayer(string name, bool isDealer, bool isAttacker)
        {
            return new Player()
            {
                Attacker = isAttacker,
                Dealer = isDealer,
                Name = name,
                Hand = new List<object>()
            };
        }

        public IEnumerable<Player> CreatePlayers(IEnumerable<string> names)
        {
            var allPlayers = new List<Player>();
            foreach(var name in names)
            {
                allPlayers.Add(CreatePlayer(name, false, false));
            }
            return allPlayers;
        }

        Contract IPlayerService.BetsContract()
        {
            throw new NotImplementedException();
        }

        public void MakeDealer(Player player)
        {
            player.Dealer = true;
        }

        public int CountCardsInHand(Player player)
        {
            return player.Hand.Count;
        }
    }
}