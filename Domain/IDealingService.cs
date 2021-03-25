namespace TarotFr.Domain
{
    interface IDealingService
    {
        bool NoMoreCardsInDeckAfterDealing();        
        bool CheckAside();
        bool CheckPlayersHaveSameNbCardsInHand();
        bool TableHasDealer();
    }
}
