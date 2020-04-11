namespace TarotFr.Domain
{
    internal interface ITarotTableService
    {
        TarotTable ShuffleDeck();
        Card Pick();       
    }
}
