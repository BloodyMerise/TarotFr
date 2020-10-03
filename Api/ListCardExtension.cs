using System;
using System.Collections.Generic;
using System.Text;
using TarotFr.Infrastructure;
using TarotFr.Domain;
using System.Linq;

namespace TarotFr.Api
{
    public static class CardExtensions
    {
        public static double Score(this IEnumerable<Card> cardsToCount)
        {
            return cardsToCount.Select(x => CardCountingRules.GetPoints(x.IsTrumper(),x.IsOudler(),x.Points())).Sum();
        }        
    }
}
