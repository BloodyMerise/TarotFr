using System;
using System.Collections.Generic;
using TarotFr.Domain;

namespace TarotFr.Infrastructure
{
    public class Card : IComparable
    {
        public enum CardColors
        {
            trumpers,
            hearts,
            spades,
            diamonds,
            clubs
        };

        public CardColors Color;
        private FaceValue _faceValue;
        private readonly double _cardScore;        
        private readonly bool _isOudler;
        private bool _isFirst;
        
        public bool IsOudler() => _isOudler;
        private bool IsFirst() => _isFirst;
        
        public bool IsTrumper() => Color.Equals(CardColors.trumpers);
        public string GetColorAsString() => Color.ToString();
        public int Points() => _faceValue.GetPoints();        
        public double Score() => _cardScore;
        
        public Card(string color, int points)
        {
            Color = (CardColors) Enum.Parse(typeof(CardColors), color, true); 
            _faceValue = new FaceValue(points);
            _isOudler = CheckOudler();
            _cardScore = CardCountingRules.GetPoints(IsTrumper(), _isOudler, _faceValue.GetPoints());
            CheckConsistency();
        }

        public override string ToString()
        {
            if (!IsTrumper())
            {
                switch (_faceValue.GetPoints())
                {
                    case 1:
                        return $"Ace of {Color}";
                    case 11:
                        return $"Jack of {Color}";
                    case 12:
                        return $"Jumper of {Color}";
                    case 13:
                        return $"Queen of {Color}";
                    case 14:
                        return $"King of {Color}";
                    default:
                        return $"{_faceValue.GetPoints()} of {Color}";
                }
            }
            else if (IsOudler())
            {
                switch (_faceValue.GetPoints())
                {
                    case 0:
                        return "Excuse";
                    case 1:
                        return "Petit";
                    case 21:
                        return "21 of trumpers";
                    default:
                        throw new ArgumentException("Cannot describe an oudler with face value other than 0,1,21");
                }
            }
            else return $"{_faceValue.GetPoints()} of {Color}";
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
            if(points > 14 && Color != CardColors.trumpers) { throw new ArgumentOutOfRangeException("_faceValue", _faceValue.GetPoints(), "Invalid nbpoints for this card"); }
            if(points == 1 || points == 21 || points == 0)
            {
                if(IsTrumper() && !CheckOudler()) { throw new ArgumentOutOfRangeException("_faceValue", _faceValue.GetPoints(), "This card should be oudler"); }
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is Card)
            {
                if (this == (Card) obj) return 0;
                if (this > (Card) obj) return 1;
                else return -1;
            }
            else throw new NotSupportedException();
        }

        public static bool operator >(Card a, Card b)
        {
            if (a == b) return false;
            else if (a.Points() == 0) return false;
            else if (b.Points() == 0) return true;
            else if (a.IsTrumper() && b.IsTrumper()) return (a.Points() > b.Points() ? true : false);
            else if (a.IsTrumper() && !b.IsTrumper()) return true;
            else if (!a.IsTrumper() && b.IsTrumper()) return false;
            else if (a.IsFirst() && !b.IsTrumper() && !a.IsTrumper()) return true;
            else if (b.IsFirst() && !b.IsTrumper() && !a.IsTrumper()) return false;
            else return (a.Points() > b.Points() ? true : false);
        }

        public static bool operator <(Card a, Card b) => (a == b) ? false : !(a > b);

        public static bool operator ==(Card a, Card b)
        {
            if (a.IsFirst() || b.IsFirst()) return false;
            if (a.Points() != b.Points()) return false;
            else
            {
                if (a.Color == b.Color) return true;
                else if (a.IsTrumper()) return false;
                else if (b.IsTrumper()) return false;
                else return true; // so 4spades == 4clubs
            }
        }

        public static bool operator !=(Card a, Card b) => !(a == b);

        public void SetIsFirst()
        {
            _isFirst = true;
        }
    }
}
