using System;
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
                Name = name,
                Hand = new List<object>(),
                Dealer = isDealer,
                Attacker = isAttacker,
                Contract = new Contract("pass"),
                WonHands = new List<object>()                
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
            DealingRules dr = new DealingRules();
            IEnumerable<Card> cardsInHand = player.Hand.Cast<Card>();

            var possibleAsideCards = cardsInHand.Where(x => dr.CardAllowedInPlayerAside(x.IsTrumper(), x.IsOudler(), x.Points())).ToList();

            if (possibleAsideCards.Count() == 0)
            {
                throw new NotSupportedException("Cannot make aside without possible cards");
            }

            var playerChoice = AskPlayerCards(player, possibleAsideCards, nbCards);
            player.WonHands.AddRange(playerChoice);
            playerChoice.ForEach(x => player.Hand.Remove(x));            
        }

        public List<Card> AskPlayerCards(Player player, IEnumerable<Card> choices, int nbCards)
        {
            if (choices.Count() < nbCards)
            {
                throw new ArgumentException("Cannot ask more cards than given choice");
            }

            List<Card> playerPicks = new List<Card>();
            Random rnd = new Random();                       

            while (nbCards > 0)
            {
                int cardIndex = rnd.Next(choices.Except(playerPicks).Count());
                playerPicks.Add(choices.Except(playerPicks).ElementAt(cardIndex));                                
                nbCards--;
            }

            return playerPicks;
        }

        public Card AskPlayerCard(Player player, IEnumerable<Card> choices)
        {                    
            if (choices.Count() < 1)
            {
                throw new ArgumentException("Cannot ask more cards than given choice");
            }

            Random rnd = new Random();            
            return choices.ElementAt(rnd.Next(choices.Count()));
        }

        public void MakeDealer(Player player)
        {
            player.Dealer = true;
        }

        public int CountCardsInHand(Player player)
        {
            return player.Hand.Count;
        }

        public bool CanDeclareHandful(Player player)
        {
            return CardCountingRules.HasHandful(player.Hand.Cast<Card>().Count(x => x.IsTrumper()));
        }

        public bool CanDeclareMisere(Player player)
        {
            return CardCountingRules.HasMisere(player.Hand.Cast<Card>().Count(x => !x.IsTrumper() && x.Points() > 10));
        }

        // Later should prompt user if he wants to disclose this
        // and implement the card disclosure as well
        public void AskForHandful(Player player)
        {
            if (CanDeclareHandful(player)) player.HasHandful = true;
        }

        // Later should prompt user if he wants to disclose this
        // and implement the card disclosure as well
        public void AskForMisere(Player player)
        {
            if (CanDeclareMisere(player)) player.HasMisere = true;
        }
    }
}
