using System;
using System.Collections.Generic;
using System.Linq;

namespace TarotFr.Domain
{
    internal class Color
    {
        private string _name;
        private bool _isTrumper;
        private static List<string> validColors = new List<string> { "hearts","spades","diamonds","clubs","trumpers" };
     
        internal bool IsTrumper() => _isTrumper;
        private bool CheckValidColor(string color) => validColors.Contains(color.ToLower());
        internal string GetColor() => _name;

        public Color(string color)
        {            
            _name = CheckValidColor(color.ToLower()) ? color.ToLower() : null;
            _isTrumper = _name == "trumpers";
        }
    }
}
