/* Author:  Lindy Stewart
 * Changes: Eric Robinson L00709820
 * Date:    10/28/23
 * Lane Community College CS234 Advanced Programming: C# (.NET)
 * Lab 3
 * Purpose: 
 */

using NUnit.Framework;

using MMABooksProps;
using System;

namespace MMABooksTests
{
    [TestFixture]
    public class CustomerPropsTests
    {
        CustomerProps props;
        [SetUp]
        public void Setup()
        // Setup() runs before every test.
        // Assign arbitrary values.
        {
            props = new CustomerProps();
            props.CustomerID = 1;
            props.Name = "Achilles";
            props.Address = "10 Aechaen Way";
            props.City = "Troy";
            props.Customer = "NY";
            props.ZipCode = "01234";
        }

        [Test]
        public void TestGetCustomer()
        {
            string jsonString = props.GetCustomer();
            Console.WriteLine(jsonString);
            Assert.IsTrue(jsonString.Contains(props.Address));
            Assert.IsTrue(jsonString.Contains(props.Name));
        }

        [Test]
        public void TestSetCustomer()
        {
            string jsonString = props.GetCustomer();
            CustomerProps newProps = new CustomerProps();
            newProps.SetCustomer(jsonString);
            Assert.AreEqual(props.CustomerID, newProps.CustomerID);
            Assert.AreEqual(props.Name, newProps.Name);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }

        [Test]
        public void TestClone()
        {
            CustomerProps newProps = (CustomerProps)props.Clone();
            Assert.AreEqual(props.CustomerID, newProps.CustomerID);
            Assert.AreEqual(props.Name, newProps.Name);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }

    } // end class CustomerPropsTests
} // end namespace MMABooksTests