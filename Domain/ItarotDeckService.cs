using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Domain
{
    internal interface ITarotDeckService
    {
        TarotDeck ShuffleDeck();
        Card Pick();       
    }
}
