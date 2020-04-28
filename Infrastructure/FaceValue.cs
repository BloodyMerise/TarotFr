using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Infrastructure
{
    internal class FaceValue
    {
        private int _pointNumber;

        internal int GetPoints() => _pointNumber;

        public FaceValue(int points)
        {
            if (points > 21 || points < 0) throw new ArgumentOutOfRangeException("points", points, "wrong number of points");
            _pointNumber = points;
        }
    }
}
