﻿using System.Collections.Generic;

namespace TarotFr.Domain
{
    public class Player
    {
        Contract _contract;
        public string Name;
        public bool Dealer { get; set; }
        public bool Attacker { get; set; }
        public List<object> Hand { get; set; }
        public IEnumerable<object> WonHands { get; set; }
    }
}