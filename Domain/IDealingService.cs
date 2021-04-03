using System.Collections.Generic;

namespace TarotFr.Domain
{
    public interface IDealingService
    {
        bool NoMoreCardsInDeckAfterDealing();        
        bool DealtAsideIsCorrect();
        bool CheckPlayersHaveSameNbCardsInHand();
        bool TableHasDealer();
        void DealsAllCardsFromDeck();
        Player AttackerCallsKing(Player attacker);
        void SendCardsToAside(IEnumerable<object> cards);
    }
}
