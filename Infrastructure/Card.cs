using System;
using System.Collections.Generic;
using TarotFr.Domain;

namespace TarotFr.Infrastructure
{
    public class Card : object
    {
        private Colors.CardColors _color;
        private FaceValue _faceValue;
        private readonly double _cardScore;
        private readonly bool _isOudler;

        public bool IsOudler() => _isOudler;
        public bool IsTrumper() => _color.Equals(Colors.CardColors.trumpers);
        public string getColor() => _color.ToString();
        public int Points() => _faceValue.GetPoints();        
        public double Score() => _cardScore;
        
        public Card(string color, int points)
        {
            _color = (Colors.CardColors) Enum.Parse(typeof(Colors.CardColors), color, true); 
            _faceValue = new FaceValue(points);
            _isOudler = CheckOudler();
            _cardScore = CardCountingRules.GetPoints(IsTrumper(), _isOudler, _faceValue.GetPoints());
            CheckConsistency();
        }

        public override string ToString()
        {
            return $"{_faceValue} of {_color}";
        }

        private bool CheckOudler()
        {
            List<int> oudlersPoints = new List<int> { 1, 21, 0 };        
            return IsTrumper() && oudlersPoints.Contains(_faceValue.GetPoints());            
        }

        private void CheckConsistency()
        {
            int points = _faceValue.GetPoints();
            if (!IsTrumper())
            {
                if (points > 14 ) throw new ArgumentOutOfRangeException("_faceValue",_faceValue.GetPoints(),"Invalid nbpoints for this card");
                if (points == 0) throw new ArgumentOutOfRangeException("_faceValue", _faceValue.GetPoints(), "Invalid nbpoints for this card");
            }
            if(points > 14 && _color != Colors.CardColors.trumpers) { throw new ArgumentOutOfRangeException("_faceValue", _faceValue.GetPoints(), "Invalid nbpoints for this card"); }
            if(points == 1 || points == 21 || points == 0)
            {
                if(IsTrumper() && !CheckOudler()) { throw new ArgumentOutOfRangeException("_faceValue", _faceValue.GetPoints(), "This card should be oudler"); }
            }
        }        
    }
}
