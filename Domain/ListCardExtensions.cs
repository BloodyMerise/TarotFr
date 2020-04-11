using System.Collections.Generic;
using System.Linq;

namespace TarotFr.Domain
{
    public static class ListCardExtensions
    {
        public static int Score(this IEnumerable<Card> cardsToCount)
        {            
            return (int) cardsToCount.Select(x => x.Score()).Sum();
        }
    }
}
