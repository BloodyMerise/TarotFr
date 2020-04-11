using System;

namespace TarotFr.Domain
{
    internal class Contract
    {
        private enum Contracts
        {
            pass = 0,
            small = 1,
            push = 2,
            guard = 3,
            guardWithout = 4,
            guardAgainst = 5            
        }

        private Contracts _contract;

        public Contract(string contractName)
        {
            if (String.IsNullOrEmpty(contractName))  _contract = Contracts.pass; 
            else _contract = (Contracts)Enum.Parse(typeof(Contracts), contractName, true);
        }

        public static bool operator >(Contract a, Contract b)
        {            
            return a._contract > b._contract;
        }

        public static bool operator <(Contract a, Contract b)
        {         
            return a._contract < b._contract;
        }

        public string[] GetNames()
        {
            return Enum.GetNames(typeof(Contracts));            
        }
    }
}
