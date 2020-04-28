using System;
using System.Collections.Generic;
using System.Text;
using TarotFr.Infrastructure;
using TarotFr.Domain;
using System.Linq;

namespace TarotFr.Api
{
    public static class ListCardExtension
    {
        public static int Score(this IEnumerable<Card> cardsToCount)
        {
            return (int)cardsToCount.Select(x => CardCountingRules.GetPoints(x.IsTrumper(),x.IsOudler(),x.Points())).Sum();
        }
    }
}
