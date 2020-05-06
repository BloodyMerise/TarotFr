using NUnit.Framework;
using TarotFr.Api;
using TarotFr.Infrastructure;
using System.Linq;
using System.Collections.Generic;

namespace TarotFr.Tests
{
    class TarotTableTest
    {
        static IEnumerable<Player> PlayerGenerator(int nbPlayers)
        {
            for(int i=0; i < nbPlayers; i++)
            { 
                yield return new Player("Prosper_" + i.ToString());                
            }            
        }

        private TarotTable CreateTable(int nbPlayers, bool startsLeft, bool needDealer)
        {
            List<Player> round = new List<Player>();

            foreach(Player player in PlayerGenerator(nbPlayers))
            {                
                round.Add(player);
            }

            if (needDealer) round.First().MakeDealer();

            return new TarotTable(true, startsLeft, round);
        }

        [Test]
        public void RoundNextPlayerChangeAccordingToRoundDirectionWithPlayer()
        {            
            TarotTable tableLeft = CreateTable(3, true, true);
            TarotTable tableRight = CreateTable(3, false, true);           

            Player dealerLeft = tableLeft.GetRoundDealer();
            Player dealerRight = tableRight.GetRoundDealer();

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
