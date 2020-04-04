using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Domain
{
    internal interface ITarotDeskService
    {
        TarotDeck ShuffleDeck();
        List<Card> SelectCards();
        Card Pick();       
    }
}
