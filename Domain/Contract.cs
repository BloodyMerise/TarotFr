using System;
using System.Collections.Generic;

namespace TarotFr.Domain
{
    public class Contract : IComparable
    {
        public enum Contracts
        {
            pass,
            small,
            push,
            guard,
            guardWithout,
            guardAgainst,
            smallChelem,
            grandChelem
        }
        
        private Contracts _contract;

        public Contract(string contractName)
        {
            if (string.IsNullOrEmpty(contractName))  _contract = Contracts.pass; 
            else _contract = (Contracts)Enum.Parse(typeof(Contracts), contractName, true);
        }

        public Contract PickRandomly(List<Contract> availableContracts)
        {
            Random rnd = new Random();
            return availableContracts[rnd.Next(availableContracts.Count)];
        }

        public static bool operator >(Contract a, Contract b)
        {            
            return a._contract > b._contract;
        }

        public static bool operator <(Contract a, Contract b)
        {         
            return a._contract < b._contract;
        }

        public static bool operator ==(Contract a, Contract b)
        {
            return a._contract == b._contract;
        }

        public static bool operator !=(Contract a, Contract b)
        {
            return a._contract != b._contract;
        }

        public static bool operator <=(Contract a, Contract b)
        {
            return a._contract <= b._contract;
        }

        public static bool operator >=(Contract a, Contract b)
        {
            return a._contract >= b._contract;
        }


        public string[] GetAll()
        {
            return Enum.GetNames(typeof(Contracts));            
        }

        public int CompareTo(object a)
        {
            if (this == (Contract) a) return 0;
            if (this < (Contract) a) return -1;
            return 1;
        }

        public override string ToString()
        {
            return _contract.ToString();
        }
    }
}
