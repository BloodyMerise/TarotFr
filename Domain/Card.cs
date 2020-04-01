using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace TarotFr.Domain
{
    public class Card
    {
        private Color _color;
        private FaceValue _faceValue;
        private CardScore _cardScore;
        private bool _isOudler;

        public bool IsOudler() => _isOudler;
        public int Points() => _faceValue.GetPoints();
        public string Color() => _color.GetColor();
        public bool IsTrumper() => _color.IsTrumper();
        public int Score() => _cardScore.GetScore();

        public Card(string color, int points)
        {
            _color = new Color(color);
            _faceValue = new FaceValue(points);
            _isOudler = checkOudler();
            _cardScore = new CardScore(_color.IsTrumper(), _isOudler, _faceValue);
            checkConsistency();
        }

        private bool checkOudler()
        {
            List<int> oudlersPoints = new List<int> { 1, 21, 0 };        
            return _color.IsTrumper() && oudlersPoints.Contains(_faceValue.GetPoints());            
        }

        private void checkConsistency()
        {
            int points = _faceValue.GetPoints();
            if (!_color.IsTrumper())
            {
                if (points > 14 ) throw new ArgumentOutOfRangeException("_faceValue",_faceValue.GetPoints(),"Invalid nbpoints for this card");
                if (points == 0) throw new ArgumentOutOfRangeException("_faceValue", _faceValue.GetPoints(), "Invalid nbpoints for this card");
            }
        }
    }
}
