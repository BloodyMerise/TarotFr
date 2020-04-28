using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Domain
{
    public static class CardDealingRules
    {
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

        public static int NbCardsToDeal() => 3;
    }
}

