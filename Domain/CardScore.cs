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
    }
}
