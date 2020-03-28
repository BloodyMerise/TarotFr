using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Domain
{
    internal class FaceValue
    {
        private int pointNumber;
        public int getPoints() => pointNumber;

        public FaceValue(int nbpoints)
        {
            this.pointNumber = nbpoints;
        }     
    }
}
