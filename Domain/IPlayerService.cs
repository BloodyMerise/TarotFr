using System.Collections.Generic;
using TarotFr.Infrastructure;

namespace TarotFr.Domain
{
    interface IPlayerService
    {
        Player CreatePlayer(string name, bool isDealer, bool isAttacker);
        void MakeDealer(Player player);
        void DealsCards(IEnumerable<Card> cards, Player player);
        void BetsContract(Player player);
    }
}
