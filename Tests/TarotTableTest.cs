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
            LinkedList<Player> round = new LinkedList<Player>();

            foreach(Player player in PlayerGenerator(nbPlayers))
            {                
                round.AddFirst(player);
            }

            if (needDealer) round.First().MakeDealer();

            return new TarotTable(true, startsLeft, round);
        }

        [Test]
        public void RoundNextPlayerChangeAccordingToRoundDirection()
        {            
            TarotTable tableLeft = CreateTable(3, true, true);
            TarotTable tableRight = CreateTable(3, false, true);

            Player dealerLeft = tableLeft.GetRoundDealer();
            Player dealerRight = tableRight.GetRoundDealer();

            Assert.AreNotEqual(tableLeft.NextPlayer(dealerLeft).Name, tableRight.NextPlayer(dealerRight).Name);
        }
    }
}
