using System;
using System.Collections.Generic;

namespace TarotFr.Domain
{
    public class DealingRules
    {
        public const int MaxCardsInDeck = 78;
        public const int NbCardsToDeal = 3;
        public int[] PossibleNbPlayers = new int[] { 3, 4, 5 };

        public int DogMaxCards(int nbPlayers)
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

