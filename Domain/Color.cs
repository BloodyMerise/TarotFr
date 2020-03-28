using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Domain
{
    internal class Color
    {
        private string colorName;

        public string GetName() => colorName;

        public Color(string color)
        {
            colorName = color;
        }        
    }
}
