using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Domain
{
    interface IPlayerService
    {
        void DealsAllCards(TarotTable deck, int step, LinkedList<Player> round, int limitDog);
        void MakeDealer();
        void MakeAttacker();
        void MakeDefenser();
        void BetsContract();
    }
}
