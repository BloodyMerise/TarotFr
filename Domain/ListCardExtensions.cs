using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TarotFr.Domain
{
    public static class ListCardExtensions
    {
        public static int Score(this List<Card> cardsToCount)
        {
            List<Card> basicCards = cardsToCount.Where(x => x.Score() == 0).ToList();
            List<Card> countedCards = new List<Card>();
            int score = 0;

            if (basicCards.Count == 0) return 0; //Chelem ?

            foreach (Card scoreCard in cardsToCount.Where(x => x.Score() > 0))
            {
                Card basic = basicCards.FirstOrDefault();

                if (!basicCards.Contains(basic))
                {
                    score -= 1;
                    basic = countedCards.First(x => x.Score() == 0);
                    basicCards.Add(countedCards.First(x => x.Score() == 0));
                }
                else basicCards.Remove(basic);

                score += scoreCard.CountScore(basic);
                countedCards.Add(basic);
                countedCards.Add(scoreCard);
            };


            while (basicCards.Count > 1)
            {
                Card card1 = basicCards.First();
                basicCards.Remove(card1);
                Card card2 = basicCards.First();
                basicCards.Remove(card2);
                score += card1.CountScore(card2);
                countedCards.Add(card1);
                countedCards.Add(card2);
            }
            return score;
        }
    }
}
