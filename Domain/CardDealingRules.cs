using System;

namespace TarotFr.Domain
{
    public static class CardDealingRules
    {
        public const int MaxCardsInDeck = 78;
        public const int NbCardsToDeal = 3;

        public static int DogMaxCards(int nbPlayers)
        {
            switch (nbPlayers)
            {
                case 3:
                case 4:
                    return 6;                    
                case 5:
                    return 3;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

