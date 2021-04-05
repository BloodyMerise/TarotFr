using NUnit.Framework;
using TarotFr.Api;
using TarotFr.Domain;
using System.Linq;
using System.Collections.Generic;
using TarotFr.Infrastructure;
using System;

namespace TarotFr.Tests
{
    class RoundServiceTests
    {
        static IEnumerable<Player> PlayerGenerator(int nbPlayers)
        {
            PlayerService ps = new PlayerService();
            for(int i=0; i < nbPlayers; i++)
            { 
                yield return ps.CreatePlayer("Prosper_" + i.ToString(),false,false);                
            }            
        }

        static IEnumerable<Tuple<int, int>> TestPlayersNbAndPosition()
        {
            DealingRules dr = new DealingRules();
            foreach (int nbPlayer in dr.PossibleNbPlayers)
            {
                for (int i = 0; i < nbPlayer; i++)
                {
                    yield return new Tuple<int, int>(nbPlayer, i);
                }
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

        [Test]
        public void RoundWithoutPlayersThrows()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() => new RoundService(true, null));
        }

        [Test]
        public void CanFindPlayer()
        {
            PlayerService ps = new PlayerService();
            Player playerIn = ps.CreatePlayer("Prosper_0", false, false);
            Player playerOut = ps.CreatePlayer("JoseADaewq", false, false);
            var players = PlayerGenerator(3).ToList();
            players.First().Dealer = true;
            RoundService round = new RoundService(true, players);

            Assert.True(round.IsPlayerIn(playerIn));
            Assert.False(round.IsPlayerIn(playerOut));
        }

        [TestCaseSource(nameof(TestPlayersNbAndPosition))]
        public void RoundFinishWithDealerAndRotateLeft(Tuple<int, int> input)
        {
            int nbPlayers = input.Item1;
            int dealerPosition = input.Item2;

            PlayerService ps = new PlayerService();
            List<Player> roundPlayers = PlayerGenerator(nbPlayers).ToList();
            RoundService round = new RoundService(true, roundPlayers);

            ps.MakeDealer(roundPlayers[dealerPosition]);

            for (int i = dealerPosition + 1; i < nbPlayers; i++)
            {
                Assert.AreEqual(roundPlayers[i].Name, round.NextPlayer().Name);
            }

            for (int i = 0; i < dealerPosition + 1; i++)
            {
                Assert.AreEqual(roundPlayers[i].Name, round.NextPlayer().Name);
            }
        }

        [TestCaseSource(nameof(TestPlayersNbAndPosition))]
        public void RoundFinishWithDealerAndRotateRight(Tuple<int, int> input)
        {
            int nbPlayers = input.Item1;
            int dealerPosition = input.Item2;

            PlayerService ps = new PlayerService();
            List<Player> roundPlayers = PlayerGenerator(nbPlayers).ToList();
            RoundService round = new RoundService(true, roundPlayers);

            ps.MakeDealer(roundPlayers[dealerPosition]);

            for (int i = dealerPosition + 1; i < nbPlayers; i++)
            {
                Assert.AreEqual(roundPlayers[i].Name, round.NextPlayer().Name);
            }

            for (int i = 0; i < dealerPosition + 1; i++)
            {
                Assert.AreEqual(roundPlayers[i].Name, round.NextPlayer().Name);
            }
        }

        [Test]
        public void RoundRotatesRightAndLeftCheck()
        {
            PlayerService ps = new PlayerService();
            List<Player> roundPlayers = PlayerGenerator(3).ToList();
            ps.MakeDealer(roundPlayers[2]);
            RoundService roundLeft = new RoundService(true, roundPlayers);
            RoundService roundRight = new RoundService(false, roundPlayers);

            string nextNameWhenLeft = roundLeft.NextPlayer().Name;
            string nextNameWhenRight = roundRight.NextPlayer().Name;

            Assert.That(nextNameWhenLeft, Is.Not.EqualTo(nextNameWhenRight));
            Assert.That(nextNameWhenLeft, Is.EqualTo("Prosper_0"));
            Assert.That(nextNameWhenRight, Is.EqualTo("Prosper_1"));
        }

        [TestCase(3, 5, 1)]
        [TestCase(4, 4, 1)]
        [TestCase(4, 7, 1)]
        [TestCase(4, 0, 0)]
        [TestCase(5, 1, 0)]
        public void RoundNumberIsCorrect(int nbPlayers, int nbTimesPlayed, int expectedNumberOfRounds)
        {
            PlayerService ps = new PlayerService();
            List<Player> roundPlayers = PlayerGenerator(nbPlayers).ToList();
            RoundService round = new RoundService(false, roundPlayers);
            ps.MakeDealer(roundPlayers[0]);

            for (int i = 0; i < nbTimesPlayed; i++)
            {
                round.NextPlayer();
            }

            Assert.AreEqual(expectedNumberOfRounds, round.RoundNumber);
        }

        [Theory]
        public void FirstRoundAlwaysStartsAfterDealer(bool startsFromLeft)
        {
            List<Player> players = PlayerGenerator(5).ToList();
            players.First().Dealer = true;
            RoundService rs = new RoundService(startsFromLeft, players);
            RoundService testRound = new RoundService(startsFromLeft, players);

            var nextPlayer = testRound.NextPlayer();

            Assert.That(rs.PlayerStartingRound(), Is.EqualTo(nextPlayer));
            Assert.That(rs.PlayerStartingRound().Dealer, Is.False);
        }

        [Test]
        public void AfterFirstRoundWinnerStarts()
        {
            int nbPlayers = 4;
            Card highestCard = new Card("trumpers", 21); // beats anything
            Card lowestCard = new Card("hearts", 1); // is beaten by anything

            List<Player> players = PlayerGenerator(nbPlayers).ToList();
            Player winner = players.First();
            winner.Dealer = true;
            
            RoundService roundService = new RoundService(false, players);
            players.ForEach(x => x.Hand.Add(lowestCard));
            winner.Hand = new List<object>() { highestCard };
    
            var plays = roundService.PlayRound();
            var firstPlayer = roundService.PlayerStartingRound();

            Assert.That(firstPlayer.Name, Is.EqualTo(winner.Name));
            Assert.Zero(winner.Hand.Count);
            Assert.That(roundService.RoundNumber, Is.EqualTo(1));
            Assert.That(plays.Count, Is.EqualTo(nbPlayers));
            Assert.That(winner.WonHands.Count, Is.EqualTo(nbPlayers));
            Assert.That(players.Sum(x => x.WonHands.Count), Is.EqualTo(nbPlayers));
        }
        
        [Test]
        public void FirstCardAlwaysWinsIfNoTrumperAndNotSameColor()
        {
            int nbPlayers = 4;
            Card firstCard = new Card("hearts", 1);
            Card otherCard = new Card("spades", 14);
            List<Player> players = PlayerGenerator(nbPlayers).ToList();
            RoundService roundService = new RoundService(false, players);

            List<Tuple<Player, Card>> roundPlay = new List<Tuple<Player, Card>>();
            foreach(var player in players)
            {
                if (roundPlay.Count == 0) roundPlay.Add(new Tuple<Player, Card>(player, firstCard));
                else roundPlay.Add(new Tuple<Player, Card>(player, otherCard));
            }

            var roundWinner = roundService.RoundWinner(roundPlay);

            Assert.That(roundWinner.Name, Is.EqualTo(roundPlay.First().Item1.Name));            
        }

        [Theory]
        public void ExcuseIsLostIfPlayedLast(bool playerWithExcuseIsAttacker)
        {
            int nbPlayers = 4;
            Card excuse = new Card("trumpers", 0);
            Card otherCard = new Card("spades", 14);
            List<Player> players = PlayerGenerator(nbPlayers).ToList();
            RoundService roundService = new RoundService(false, players);

            foreach(var player in players)
            {
                player.Hand.Add(otherCard);                
            }

            //need one attacker, to check that the excuse changes team
            if (!playerWithExcuseIsAttacker) { players.Last().Attacker = true; }

            var playerWithExcuse = players.First();
            playerWithExcuse.Hand = new List<object>() { excuse };
            playerWithExcuse.Attacker = playerWithExcuseIsAttacker;
            playerWithExcuse.Dealer = true;

            roundService.PlayRound();

            Assert.That(playerWithExcuse.WonHands.Contains(excuse), Is.False);
            Assert.That(players.Count(x => x.WonHands.Contains(excuse)), Is.EqualTo(1));
            Assert.That(players.Where(x => x.WonHands.Contains(excuse)).Select(x => x.Attacker).First(), Is.EqualTo(!playerWithExcuseIsAttacker));
        }
    }
}
