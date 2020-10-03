using System;

namespace TarotFr.Domain
{
    class Contract
    {
        enum Contracts
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
        /*
        private Contracts _contract;

        public Contract(string contractName)
        {
            if (String.IsNullOrEmpty(contractName))  _contract = Contracts.pass; 
            else _contract = (Contracts)Enum.Parse(typeof(Contracts), contractName, true);
        }

        public Contract PickContract()
        {
            Random rnd = new Random();
            return new Contract((string) Enum.GetValues(typeof(Contracts)).GetValue(rnd.Next(0, 6)));
        }

        public static bool operator >(Contract a, Contract b)
        {            
            return a._contract > b._contract;
        }

        public static bool operator <(Contract a, Contract b)
        {         
            return a._contract < b._contract;
        }

        public string[] GetAll()
        {
            return Enum.GetNames(typeof(Contracts));            
        }*/
    }
}
