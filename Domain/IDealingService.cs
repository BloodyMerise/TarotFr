namespace TarotFr.Domain
{
    interface IDealingService
    {
        bool NoMoreCardsInDeckAfterDealing();        
        bool CheckDog();
        bool CheckPlayersHaveSameNbCardsInHand();
        bool TableHasDealer();
    }
}
