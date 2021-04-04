using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TarotFr.Infrastructure;
using TarotFr.Domain;
using TarotFr.Api;
using System.Linq;

namespace TarotFrTests.Api
{
    class PlayableCardsTest
    {
        // Except for onlyOudlers, hearts is always the first color played
        static IEnumerable<List<Card>> TestCards(bool onlyBasics, bool onlyOudlers)
        {
            if (onlyBasics)
            {
                yield return new List<Card>()
                {
                    new Card("hearts", 3), 
                    new Card("clubs", 1),
                    new Card("diamonds", 10),
                    new Card("spades", 14)
                };
            }
            else if (onlyOudlers)
            {
                yield return new List<Card>()
                {
                    new Card("trumpers", 1),
                    new Card("trumpers", 21),
                    new Card("trumpers", 0)
                };
            }
            else
            {
                yield return new List<Card>()
                {
                    new Card("hearts", 3),
                    new Card("spades", 14),
                    new Card("clubs", 1),
                    new Card("diamonds", 10),
                    new Card("trumpers", 3),
                    new Card("trumpers", 1),
                    new Card("trumpers", 21)
                };                    
            }
        }

        [TestCaseSource(nameof(TestCards), new object[] { false, false })]
        [TestCaseSource(nameof(TestCards), new object[] { true, false })]
        [TestCaseSource(nameof(TestCards), new object[] { false, true})]
        public void CanAlwaysPlayExcuse(List<Card> playedCards)
        {
            PlayerService ps = new PlayerService();
            Player player = ps.CreatePlayer("ASD",true,true);
            Card excuse = new Card("trumpers", 0);
            player.Hand.Add(excuse);
            RoundService rs = new RoundService(true, new List<Player>() { player });

            List<Card> playableCards = rs.GetPlayableCardsForPlayer(player, playedCards).ToList();

            Assert.That(playableCards.Count,Is.EqualTo(1));
            Assert.That(playableCards.First(), Is.EqualTo(excuse));
        }
        
        [TestCaseSource(nameof(TestCards), new object[] { true, false })]
        public void CanPlayTrumperOnlyIfDontHaveSameColorInHand(List<Card> playedCards)
        {
            PlayerService ps = new PlayerService();
            Player player = ps.CreatePlayer("ASD", true, true);            
            RoundService rs = new RoundService(true, new List<Player>() { player });
            Card queenHearts = new Card("hearts", 13);
            Card eighteenTrumper = new Card("trumpers", 18);

            player.Hand = new List<object>
            {
                eighteenTrumper,
                queenHearts,
                new Card("spades",1)
            };

            List<Card> playableCards = rs.GetPlayableCardsForPlayer(player, playedCards).ToList();

            Assert.That(playableCards.Count, Is.EqualTo(1));
            Assert.That(playableCards.First(), Is.EqualTo(queenHearts));

            player.Hand.Remove(queenHearts);
            playableCards = rs.GetPlayableCardsForPlayer(player, playedCards).ToList();

            Assert.That(playableCards.Count, Is.EqualTo(1));
            Assert.That(playableCards.First(), Is.EqualTo(eighteenTrumper));
        }

        [TestCaseSource(nameof(TestCards), new object[] { false, false })]
        [TestCaseSource(nameof(TestCards), new object[] { false, true })]
        public void MustPlayLowerTrumperIfHasAny(List<Card> playedCards)
        {
            PlayerService ps = new PlayerService();
            Player player = ps.CreatePlayer("ASD", true, true);
            RoundService rs = new RoundService(true, new List<Player>() { player });
            Card eighteenTrumper = new Card("trumpers", 18);

            player.Hand = new List<object>
            {
                eighteenTrumper,                
                new Card("spades",1)
            };

            List<Card> playableCards = rs.GetPlayableCardsForPlayer(player, playedCards).ToList();

            Assert.That(playableCards.Count, Is.EqualTo(1));
            Assert.That(playableCards.First(), Is.EqualTo(eighteenTrumper));            
        }

        [TestCaseSource(nameof(TestCards), new object[] { false, false })]
        [TestCaseSource(nameof(TestCards), new object[] { false, true })]
        public void MustPlayBasicIfNoMoreTrumperAndNoFirstColorInHand(List<Card> playedCards)
        {
            PlayerService ps = new PlayerService();
            Player player = ps.CreatePlayer("ASD", true, true);
            RoundService rs = new RoundService(true, new List<Player>() { player });
            
            player.Hand = new List<object>
            {
                new Card("clubs", 8),
                new Card("spades",1)
            };

            List<Card> playableCards = rs.GetPlayableCardsForPlayer(player, playedCards).ToList();

            Assert.That(playableCards.Count, Is.EqualTo(2));
            Assert.That(playableCards, Is.EqualTo(player.Hand));
        }
        
        [Test]
        public void MustPlayHigherTrumperIfTrumperPlayed()
        {
            PlayerService ps = new PlayerService();
            Player player = ps.CreatePlayer("ASD", true, true);
            RoundService rs = new RoundService(true, new List<Player>() { player });
            Card eighteenTrumper = new Card("trumpers", 18);

            player.Hand = new List<object>
            {
                new Card("trumpers", 8),
                eighteenTrumper,
                new Card("spades",14)            
            };

            List<Card> playedCards = new List<Card>()
            {
                new Card("hearts",14),
                new Card("trumpers", 9),                                
            };

            List<Card> playableCards = rs.GetPlayableCardsForPlayer(player, playedCards).ToList();

            Assert.That(playableCards.Count, Is.EqualTo(1));
            Assert.That(playableCards.First(), Is.EqualTo(eighteenTrumper));
        }
    }
}
