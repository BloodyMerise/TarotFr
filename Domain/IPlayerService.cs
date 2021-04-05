using System.Collections.Generic;
using TarotFr.Infrastructure;

namespace TarotFr.Domain
{
    interface IPlayerService
    {
        Player CreatePlayer(string name, bool isDealer, bool isAttacker);
        void MakeDealer(Player player);
        void DealsCards(IEnumerable<Card> cards, Player player);
        Contract AskForBet(Player player, List<Contract> choices);
        void SetTargetScore(Player player);
        bool CanDeclareHandful(Player player);
        bool CanDeclareMisere(Player player);
        void AskForHandful(Player player);
        void AskForMisere(Player player);
    }
}
