using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Domain
{
    internal interface ItarotDeckService
    {
        TarotDeck ShuffleDeck();
        List<Card> SelectCards();
        Card Pick();       
    }
}
