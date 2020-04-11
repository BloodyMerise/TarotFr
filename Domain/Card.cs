using System;
using System.Collections.Generic;

namespace TarotFr.Domain
{
    public class Card
    {
        internal enum CardColors
        {
            hearts,
            spades,
            diamonds,
            clubs,
            trumpers            
        };

        private CardColors _color;
        private FaceValue _faceValue;
        private CardScore _cardScore;
        private bool _isOudler;

        public bool IsOudler() => _isOudler;
        public string getColor() => _color.ToString();
        public int Points() => _faceValue.GetPoints();
        public bool IsTrumper() => _color == CardColors.trumpers;
        public decimal Score() => _cardScore.GetScore();
        public int CountScore(Card card) => _cardScore.CountScore(card);

        public Card(string color, int points)
        {
            _color = (CardColors)Enum.Parse(typeof(CardColors), color, true);
            _faceValue = new FaceValue(points);
            _isOudler = checkOudler();
            _cardScore = new CardScore(IsTrumper(), _isOudler, _faceValue);
            checkConsistency();
        }

        private bool checkOudler()
        {
            List<int> oudlersPoints = new List<int> { 1, 21, 0 };        
            return IsTrumper() && oudlersPoints.Contains(_faceValue.GetPoints());            
        }

        private void checkConsistency()
        {
            int points = _faceValue.GetPoints();
            if (!IsTrumper())
            {
                if (points > 14 ) throw new ArgumentOutOfRangeException("_faceValue",_faceValue.GetPoints(),"Invalid nbpoints for this card");
                if (points == 0) throw new ArgumentOutOfRangeException("_faceValue", _faceValue.GetPoints(), "Invalid nbpoints for this card");
            }
        }

        public static bool operator >(Card a, Card b)
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
            else {
                if (a._color == b._color) return true;
                else if (a.IsTrumper()) return false;
                else if (b.IsTrumper()) return false;
                else return true; // so 4spades == 4clubs
            }
        }

        public static bool operator !=(Card a, Card b) => !(a == b );
        
        public override string ToString()
        {
            return _color + _faceValue.GetPoints().ToString();
        }
    }
}
