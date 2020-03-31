using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Domain
{
    class CardService : ICardService
    {
        public Card CreateCard(string color, int faceValue)
        {
            return new Card(color, faceValue);
        }

        public Card FightCard(Card opponent)
        {
            throw new NotImplementedException();
        }

        public string ShowCard(Card card)
        {
            return card.ToString();
        }

        /*public int CountCard(Card card1, Card card2)
        {
            return card1.Score + card2.Score;
        }*/


       /* public Card FightCard(Card one, Card two)
        {
            if (one > two) return one;
            else return two;
        }*/
    }
}
