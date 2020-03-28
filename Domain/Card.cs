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
        private bool _isOudler;
        private bool _isTrumper;

        public bool IsOudler() => _isOudler;
        public bool IsTrumper() => _isTrumper;

        public Card(string color, int points)
        {
            _color = new Color(color);
            _faceValue = new FaceValue(points);
            _isOudler = checkOudler();
            _isTrumper = checkTrumper();
        }

        public string Show()
        {
            return _faceValue.getPoints().ToString() + _color.GetName();
        }

        public Card Fight(Card opponentCard)
        {
            return ResolveFight(opponentCard);            
        }

        private Card ResolveFight(Card opposingCard)
        {
            //Card resultColorFight = CompareColors(opposingCard);
            return (opposingCard.ComputePoints() > this.ComputePoints() ? opposingCard : this);
        }

        private int ComputePoints()
        {
            return _faceValue.getPoints();
        }

        private bool checkOudler()
        {
            if(_color.GetName() == "trumps")
            {
                if (_faceValue.getPoints() == 1) return true;
                if (_faceValue.getPoints() == 21) return true;
                if (_faceValue.getPoints() == 0) return true;

                return false;                
            }
            else { return false; }
        }

        private bool checkTrumper()
        {
            if (_color.GetName() == "trumps")
            {
                if (_faceValue.getPoints() >= 0 && _faceValue.getPoints() <= 21) return true;
                return false;
            }
            else { return false; }
        }
    }
}
