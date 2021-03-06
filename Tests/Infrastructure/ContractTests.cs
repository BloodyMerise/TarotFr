﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using TarotFr.Domain;

namespace TarotFrTests
{
    [TestFixture]
    public class ContractTests
    {
        [Test]
        public void PickContractReturnsContract()
        {
            Contract contractTest = new Contract(null);
            List<Contract> choices = new List<Contract>()
            {
                new Contract("pass"),
                new Contract("Guard"),
                new Contract("GuardAgainst")
            };

            Assert.That(choices.Contains(contractTest.PickRandomly(choices)));
        }

        [Test]
        public void CreateInexistingContractThrows()
        {
            Assert.Throws<ArgumentException>(() => new Contract("asda"));
        }

        [TestCase("pass","guard",false)]
        [TestCase("guard","guard",false)]
        [TestCase("guard","pass",true)]
        public void ContractGreaterLesser(string contractName1, string contractName2, bool expected)
        {
            Contract contract1 = new Contract(contractName1);
            Contract contract2 = new Contract(contractName2);

            Assert.That(contract1 > contract2, Is.EqualTo(expected));
            Assert.That(contract1 <= contract2, Is.EqualTo(!expected));
        }

        [TestCase("pass", "guard", false)]
        [TestCase("guard", "guard", true)]
        public void ContractEquality(string contractName1, string contractName2, bool expected)
        {
            Contract contract1 = new Contract(contractName1);
            Contract contract2 = new Contract(contractName2);

            Assert.That(contract1 == contract2, Is.EqualTo(expected));
            Assert.That(contract1 != contract2, Is.EqualTo(!expected));
        }

    }
}
