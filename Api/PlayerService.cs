﻿using System;
using System.Collections.Generic;
using System.Linq;
using TarotFr.Domain;
using TarotFr.Infrastructure;

namespace TarotFr.Api
{
    public class PlayerService : IPlayerService
    {    
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
                Hand = new List<object>(),
                Contract = new Contract("pass")
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

        public Contract AskForBet(Player player, List<Contract> choices)
        {
           return player.Contract.PickRandomly(choices);            
        }

        public void MakeAside(Player player, int nbCards)
        {
            player.WonHands = new List<object>();
            DealingRules dr = new DealingRules();
            IEnumerable<Card> cardsInHand = player.Hand.Cast<Card>();

            var possibleAsideCards = cardsInHand.Where(x => dr.CardAllowedInAside(x.IsTrumper(), x.IsOudler(), x.Points())).ToList();

            if (possibleAsideCards.Count() == 0)
            {
                throw new NotSupportedException("Cannot make aside without possible cards");
            }

            var playerChoice = AskPlayerCard(player, possibleAsideCards, nbCards);
            player.WonHands.AddRange(playerChoice);
            playerChoice.ForEach(x => player.Hand.Remove(x));            
        }

        public List<Card> AskPlayerCard(Player player, IEnumerable<Card> choices, int nbCards)
        {
            List<Card> playerPicks = new List<Card>();
            Random rnd = new Random();
            
            if(choices.Count() < nbCards)
            {
                throw new ArgumentException("Cannot ask more cards than given choice");
            }

            while (nbCards > 0)
            {
                int cardIndex = rnd.Next(choices.Except(playerPicks).Count());

                playerPicks.Add(choices.Except(playerPicks).ElementAt(cardIndex));
                                
                nbCards--;
            }

            return playerPicks;
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
