using System;
using System.Collections.Generic;
using System.Text;
using TarotFr.Domain;

namespace TarotFr.Api
{
    class CardService : ICardService
    {
         public string ShowCard(Card card)
        {
            return card.ToString();
        }

        /*public int CountCard(Card card1, Card card2)
        {
            return card1.Score + card2.Score;
        }*/

       public Card FightCard(Card one, Card two)
        {
            if (one > two) return one;
            else return two;
        }
    }
}
