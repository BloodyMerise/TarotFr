using NUnit.Framework;
using TarotFr.Api;
using TarotFr.Domain;
using System.Linq;
using System.Collections.Generic;
using TarotFr.Infrastructure;
using System;

namespace TarotFr.Tests
{
    class TarotTableTest
    {
        static IEnumerable<Player> PlayerGenerator(int nbPlayers)
        {
            PlayerService ps = new PlayerService();
            for(int i=0; i < nbPlayers; i++)
            { 
                yield return ps.CreatePlayer("Prosper_" + i.ToString(),false,false);                
            }            
        }

        private TarotTable CreateTable(int nbPlayers, bool startsLeft, bool needDealer)
        {
            List<Player> round = new List<Player>();

            foreach(Player player in PlayerGenerator(nbPlayers))
            {                
                round.Add(player);
            }

            if (needDealer) round.First().Dealer = true;

            return new TarotTable(true, startsLeft, round);
        }

        [Test]
        public void RoundNextPlayerChangeAccordingToRoundDirectionWithPlayer()
        {            
            TarotTable tableLeft = CreateTable(3, true, true);
            TarotTable tableRight = CreateTable(3, false, true);           

            Player dealerLeft = tableLeft.GetDealer();
            Player dealerRight = tableRight.GetDealer();

            Assert.AreNotEqual(tableLeft.NextPlayer(dealerLeft).Name, tableRight.NextPlayer(dealerRight).Name);
        }

        [Test]
        public void RoundNextPlayerChangeAccordingToRoundDirectionWithoutPlayer()
        {
            TarotTable tableLeft = CreateTable(3, true, true);
            TarotTable tableRight = CreateTable(3, false, true);

            Assert.AreNotEqual(tableLeft.NextPlayer().Name, tableRight.NextPlayer().Name);
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void RoundNumberIsCorrect(int nbPlayers)
        {
            TarotTable tb = CreateTable(nbPlayers, false, true);
            int n = 0;
            
            while(n < nbPlayers)
            {
                Assert.That(tb.GetRoundNumber() == n);

                for(int i = 0; i < nbPlayers; i++)
                {
                    tb.NextPlayer();
                }

                n++;
            }                        
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void SendAsideToWinnerMakesPlayerHaveMoreCardThanOthers(int nbPlayers)
        {
            TarotTable tb = CreateTable(nbPlayers, false, true);
            DealingService ds = new DealingService(tb);
            ds.DealsAllCardsFromDeck();
            Player attacker = tb.NextPlayer();
            attacker.Attacker = true;

            tb.SendAsideToPlayerHand(attacker);

            while(tb.GetRoundNumber() == 0)
            {
                Player player = tb.NextPlayer();
                Assert.That(attacker.Hand.Count, Is.GreaterThan(player.Hand.Count));
            }            
        }
        
        public void AsideCannotContainForbiddenCards()
        {
            PlayerService ps = new PlayerService();
            Player player = ps.CreatePlayer("asda", false, true);
            List<object> cards = new List<object>
            {
               new Card("trumpers", 3) as object,
                new Card("trumpers", 21) as object,
                new Card("clubs", 14) as object,
                new Card("diamonds", 14) as object
            };

            player.Hand = cards;            
            Assert.Throws<NotSupportedException>(() => ps.MakeAside(player, 4));
        }
    }
}
