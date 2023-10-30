/* Author:  LindyStewart
 * Changes: Eric Robinson L00709820
 * Date:    10/29/23
 * Lane Community College CS234 Advanced Programming: C# (.NET)
 * Lab 3
 * Purpose: 
 */

using NUnit.Framework;
using MMABooksBusiness;
using MMABooksProps;
using MMABooksDB;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;
using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;

namespace MMABooksTests
{
    [TestFixture]
    internal class CustomerTests
    {
        [SetUp]

        public void TestResetDatabase()
        // This method runs before every test.
        {
            CustomerDB db = new CustomerDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetCustomerData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestNewCustomerConstructor()
        {
            Customer c = new Customer();
            Assert.AreEqual(string.Empty, c.Name);
            Assert.AreEqual(string.Empty, c.Address);
            Assert.IsTrue(c.IsNew);
            Assert.IsFalse(c.IsValid);
        }

        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // Retrieves from Data Store.
            Customer c = new Customer(1);
            Assert.AreEqual("Molunguri, A", c.Name);
            Assert.AreEqual("1108 Johanna Bay Drive", c.Address);
            Assert.IsFalse(c.IsNew);
            Assert.IsTrue(c.IsValid);
        }

        [Test]
        public void TestSaveToDataStore()  
        {
            // This SHOULD create a new customer.
            // We want to make sure that it gets saved to the DB.
            // The business object (?) has a Save() method that is
            // smart enought to know if it should do an
            // Insert, Update or Delete(?) depending on the Customer of the  object. 
            Customer c = new Customer();
            c.Name = "Aneas";
            c.Address = "101 Colossuem Way";
            c.City = "Rome";
            c.State = "NY";
            c.ZipCode = "00000";
            c.Save();
            Customer c2 = new Customer(c.CustomerID); // Where does CustomerID come from?
            Assert.AreEqual(c2.Name, c.Name);
            Assert.AreEqual(c2.Address, c.Address);
        }

        [Test]
        public void TestUpdate()
        {
            Customer c = new Customer();
            c.Name = "Helen";
            c.Address = "101 Palace";
            c.City = "Sparta";
            c.State = "OH";
            c.ZipCode = "99999";
            c.Save();

            Customer c2 = new Customer();
            c2.Name = "Helen";
            c2.Address = "101 Palace";
            c2.City = "Sparta";
            c2.State = "OH";
            c2.ZipCode = "99999";
            c2.Save();
            Assert.AreEqual(c.Name, c2.Name);
            Assert.AreEqual(c2.Address, c.Address);
        }

        [Test]
        public void TestDelete()
        {
            Customer c = new Customer();
            c.Delete();
            c.Save();
            // I really have no idea how the next line works.
            Assert.Throws<Exception>(() => new Customer());
        }

        [Test]
        public void TestGetList()
        {
            Customer c = new Customer();
            List<Customer> customers = (List<Customer>)c.GetList();
            Assert.AreEqual("Molunguri, A", customers[1].Name);
            Assert.AreEqual("35216-6909", customers[1].ZipCode);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Customer c = new Customer();
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            // Another thest that I don't understand.
            Customer c = new Customer();
            Assert.Throws<Exception>(() => c.Save());
            c.Name = "??";
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestInvalidPropertySet()
        {
            Customer c = new Customer();
            Assert.Throws<ArgumentOutOfRangeException>(() => c.State = "???");
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            Customer c1 = new Customer();
            Customer c2 = new Customer();

            c1.Name = "Updated first";
            c1.Address = "1st Street";
            c1.City = "OneTown";
            c1.State = "AL";
            c1.ZipCode = "11111";
            c1.Save();

            c2.Name = "Updated second";
            c2.Address = "2nd Street";
            c2.City = "TwoTown";
            c2.State = "AK";
            c2.ZipCode = "22222";
            c2.Save();

            Assert.Throws<Exception>(() => c2.Save()); // Huh???
        }

    } // end class CustomerTests
} // end namespace MMABooksTests
