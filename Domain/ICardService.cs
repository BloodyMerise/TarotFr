using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Domain
{
    internal interface ICardService
    {
        Card CreateCard(string color, int faceValue);
        string ShowCard(Card card);
        Card FightCard(Card opponent);
    }
}
