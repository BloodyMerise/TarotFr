using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Domain
{
    class CardWinningRules
    {
        /*public static bool operator >(Card a, Card b)
        {
            if (a == b) return false;
            else if (a.Points() == 0) return false;
            else if (b.Points() == 0) return true;
            else if (a.IsTrumper() && b.IsTrumper()) return (a.Points() > b.Points() ? true : false);
            else if (a.IsTrumper() && !b.IsTrumper()) return true;
            else if (!a.IsTrumper() && b.IsTrumper()) return false;
            else return (a.Points() > b.Points() ? true : false);
        }

        public static bool operator <(Card a, Card b) => (a == b) ? false : !(a > b);

        public static bool operator ==(Card a, Card b)
        {
            if (a.Points() != b.Points()) return false;
            else
            {
                if (a._color == b._color) return true;
                else if (a.IsTrumper()) return false;
                else if (b.IsTrumper()) return false;
                else return true; // so 4spades == 4clubs
            }
        }

        public static bool operator !=(Card a, Card b) => !(a == b);

        public override string ToString()
        {
            return _color + _faceValue.GetPoints().ToString();
        }*/
    }
}
