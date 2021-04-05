using System.Collections.Generic;

namespace TarotFr.Domain
{
    public class Player
    {
        public string Name;
        public bool Dealer { get; set; }
        public bool Attacker { get; set; }
        public List<object> Hand { get; set; }
        public List<object> WonHands { get; set; }
        public override string ToString() => Name;
        public Contract Contract { get; set; }
        public double TargetScore;
        public bool HasHandful;
        public bool HasMisere;
    }
}
