using System;
using System.Collections.Generic;
using System.Linq;
using TarotFr.Domain;
using TarotFr.Infrastructure;

namespace TarotFr.Api
{
    public class DealingService : IDealingService
    {
        DealingRules _rules;
        TarotDeck _deck;
        PlayerService _ps;
        List<Card> _aside;
        private RoundService _round;

        public int NbPlayers() => _round.GetNbPlayers();
        public int GetRoundNumber() => _round.RoundNumber;
        public void ResetRoundNumber() => _round.ResetRoundNumber();

        public int CountAside() => _aside.Count;
        public bool DealtAsideIsCorrect() => CountAside() == _rules.AsideMaxCards(NbPlayers());
        public int NbCardsInAside() => _rules.AsideMaxCards(NbPlayers());

        bool IDealingService.NoMoreCardsInDeckAfterDealing() => _deck.IsEmpty() && _ps.CountCardsInHand(NextPlayer()) > 0;
        
        public DealingService(bool startsFromLeft, List<Player> players)
        {
            _ps = new PlayerService();
            _deck = new TarotDeck(true);
            _rules = new DealingRules();            
            _aside = new List<Card>();
            _round = new RoundService(startsFromLeft, players);
        }
        
        public void DealsAllCardsFromDeck()
        {
            Player dealer = GetDealer();            
                        
            while (!_deck.IsEmpty())
            {
                _ps.DealsCards(_deck.Pop(DealingRules.NbCardsToDeal), NextPlayer());
                SendCardsToAside(PickCardsForAside(_deck, CountAside()));                
            }

            ResetRoundNumber();
        }      

        private IEnumerable<Card> PickCardsForAside(TarotDeck tarotDeck, int asideCardsCount)
        {            
            int nbRemainingCardsForAside = _rules.AsideMaxCards(NbPlayers()) - asideCardsCount;           

            if (nbRemainingCardsForAside > 0)
            {
                Random rnd = new Random();
                int rndNbCardsToSend = rnd.Next(1, DealingRules.NbCardsToDeal) + 1;

                return tarotDeck.Pop(Math.Min(rndNbCardsToSend, nbRemainingCardsForAside));
            }
            else return null;
        }

        bool IDealingService.CheckPlayersHaveSameNbCardsInHand()
        {
            throw new NotImplementedException();
        }

        public void SendCardsToAside(IEnumerable<object> cards)
        {
            if (!(cards is null)) _aside.AddRange(cards as IEnumerable<Card>);
        }

        public bool TableHasDealer()
        {
            try
            {
                GetDealer();
            }
            catch (NullReferenceException)
            {                
                return false;
            }
            return true;
        }
        
        public void SendAsideToPlayerHand(Player player)
        {
            player.Hand.AddRange(_aside);
            _aside.Clear();
        }

        public Player AttackerCallsKing(Player player)
        {
            if(NbPlayers() != 5)
            {
                throw new ArgumentException("Can only call a king with 5 players");
            }
        
            if (player.Attacker is false)
            {
                throw new ArgumentException("Only an attacker can call a king");
            }

            var kings = new List<Card> {
                new Card("hearts",14),
                new Card("diamonds",14),
                new Card("clubs",14),
                new Card("spades",14)
            };

            Card calledKing = _ps.AskPlayerCard(player, kings);
            Player calledPlayer = _round.FindPlayerWithCard(calledKing);

            if (calledPlayer is null)
            {
                return player;
            }
            else
            {
                calledPlayer.Attacker = true;
                return calledPlayer;
            }
        }

        public Player NextPlayer(Player player)
        {
            return _round.NextPlayer(player);
        }

        public Player NextPlayer()
        {
            return _round.NextPlayer();
        }

        public Player GetDealer()
        {
            return _round.FindDealer();
        }
    }
}
