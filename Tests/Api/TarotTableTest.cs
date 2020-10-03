using NUnit.Framework;
using TarotFr.Api;
using TarotFr.Domain;
using System.Linq;
using System.Collections.Generic;

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
    }
}
