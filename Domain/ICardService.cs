using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Domain
{
    internal interface ICardService
    {
        string ShowCard(Card card);
        Card FightCard(Card a, Card b);       
    }
}
