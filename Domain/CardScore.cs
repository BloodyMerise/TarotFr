using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Domain
{
    internal class CardScore
    {
        private int _score;
        internal int GetScore() => _score;

        public CardScore(bool trumper, bool oudler, FaceValue fv)
        {
            if (oudler) { _score = 5; }
            else if (trumper) { _score = 0; }
            else if (fv.GetPoints() >= 11) { _score = fv.GetPoints() - 9; }
            else _score = 0;
        }

        internal int CountScore(Card card)
        {
            CheckOneCardHas0Score(card);
            if (_score == 0 && card.Score() == 0) return 1;
            return (_score > card.Score()) ? _score : card.Score();
        }

        private void CheckOneCardHas0Score(Card card)
        {
            if (card.Score() != 0 && _score != 0)
            {
                throw new ArgumentException("Cards must be counted in pairs, one with score of 0", "card");
            }
        }
    }
}
