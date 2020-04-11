using System;

namespace TarotFr.Domain
{
    internal class CardScore
    {
        private decimal _score;
        internal decimal GetScore() => _score;

        public CardScore(bool trumper, bool oudler, FaceValue fv)
        {
            if (oudler) { _score = 4.5M; }
            else if (trumper) { _score = 0.5M; }
            else if (fv.GetPoints() >= 11) { _score = fv.GetPoints() - 9.5M; }
            else _score = 0.5M;
        }

        internal int CountScore(Card card)
        {
            CheckOneCardHas0Score(card);
            return (int)(_score + card.Score());
        }

        private void CheckOneCardHas0Score(Card card)
        {
            if (card.Score() != 0.5M && _score != 0.5M)
            {
                throw new ArgumentException("Cards must be counted in pairs, one with score of 0", "card");
            }
        }
    }
}
