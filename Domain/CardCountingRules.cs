namespace TarotFr.Domain
{
    public static class CardCountingRules
    {
        public const double MaxScore = 91;
        private const double OudlerScore = 4.5;
        private const double TrumperScore = 0.5;
        
        private static double BasicScore(int cardRank)
        {
            return (cardRank > 10) ? cardRank - 9.5 : 0.5;
        }

        public static double GetPoints(bool trumper, bool oudler, int rank)
        {
            if (oudler) return OudlerScore;
            else if (trumper) return TrumperScore;
            else return BasicScore(rank);
        }            
    }
}
