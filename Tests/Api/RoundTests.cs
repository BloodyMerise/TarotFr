using NUnit.Framework;
using TarotFr.Api;
using TarotFr.Domain;
using System.Linq;
using System.Collections.Generic;
using TarotFr.Infrastructure;
using System;

namespace TarotFr.Tests
{
    class RoundTests
    {
        static IEnumerable<Player> PlayerGenerator(int nbPlayers)
        {
            PlayerService ps = new PlayerService();
            for(int i=0; i < nbPlayers; i++)
            { 
                yield return ps.CreatePlayer("Prosper_" + i.ToString(),false,false);                
            }            
        }

        [Test]
        public void RoundNextPlayerChangeAccordingToRoundDirectionWithPlayer()
        {
            var players = PlayerGenerator(3).ToList();
            players.First().Dealer = true;

            DealingService dsLeft = new DealingService(true,players);
            DealingService dsRight = new DealingService(false, players);
            
            Assert.AreNotEqual(dsLeft.NextPlayer(players.First()).Name, dsRight.NextPlayer(players.First()).Name);
        }

        [Test]
        public void RoundNextPlayerChangeAccordingToRoundDirectionWithoutPlayer()
        {
            var players = PlayerGenerator(3).ToList();
            players.First().Dealer = true;
            DealingService dsLeft = new DealingService(true, players);
            DealingService dsRight = new DealingService(false, players);

            Assert.AreNotEqual(dsLeft.NextPlayer().Name, dsRight.NextPlayer().Name);
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void RoundNumberIsCorrect(int nbPlayers)
        {
            var players = PlayerGenerator(nbPlayers).ToList();
            players.First().Dealer = true;
            DealingService ds = new DealingService(true, players);

            int n = 0;
            
            while(n < nbPlayers)
            {
                Assert.That(ds.GetRoundNumber() == n);

                for(int i = 0; i < nbPlayers; i++)
                {
                    ds.NextPlayer();
                }

                n++;
            }                        
        }        
    }
}
