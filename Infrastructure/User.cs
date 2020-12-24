using System;

namespace TarotFr.Infrastructure
{
    public class User
    {        
        public string Name { get; }

        public User(string name)
        {
            if (String.IsNullOrEmpty(name)) name = "LeGrandMamamouchi";
            else Name = name;        
        }
    }
}
