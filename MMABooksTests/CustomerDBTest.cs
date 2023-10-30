/* Author:  LindyStewart
 * Changes: Eric Robinson L00709820
 * Date:    10/25/23
 * Lane Community College CS234 Advanced Programming: C# (.NET)
 * Lab 3
 * Purpose: 
 */

using NUnit.Framework;
using MMABooksProps;
using MMABooksDB;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;
using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace MMABooksTests
{
    [TestFixture]
    internal class CustomerDBTest
    {
        CustomerDB db;

        [SetUp]
        // This method runs before every test.
        public void ResetData()
        {
            db = new CustomerDB (); // How is our .JSON file referenced to get MySQL DB password?
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieve()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(1); // We cast as CustomerProps.
            Assert.AreEqual(1, p.CustomerID);
            Assert.AreEqual("Molunguri, A", p.Name); 
        }

        /*
        [Test]
        public void TestRetrieveAll()
        {
            List<CustomerProps> list = (List<CustomerProps>)db.RetrieveAll(); // Cast as List<CustomerProps>.
            Assert.AreEqual(53, list.Count);
        }

        [Test]
        public void TestDelete()
        {
            CustomerProps p = (CustomerProps)db.Retrieve("HI"); // We cast as CustomerProps.

            // The Delete() method should return TRUE if 1 row has been deleted.
            Assert.True(db.Delete(p));

            // We just deleted "HI". If we try to retrieve it, we should get an error.
            Assert.Throws<Exception>(() => db.Retrieve("HI"));
        }

        [Test]
        public void TestDeleteForeignKeyConstraint()
        {
            CustomerProps p = (CustomerProps)db.Retrieve("OR");
            Assert.Throws<MySqlException>(() => db.Delete(p));
        }

        [Test]
        public void TestUpdate()
        {
            CustomerProps p = (CustomerProps)db.Retrieve("CA");
            p.Name = "Canada";
            Assert.True(db.Update(p));
            p = (CustomerProps)db.Retrieve("CA");
            Assert.AreEqual("Canada", p.Name);
        }

        [Test]
        public void TestUpdateFieldTooLong()
        {
            CustomerProps p = (CustomerProps)db.Retrieve("OR");
            p.Name = "Oregon is the Customer where Crater Lake National Park is.";
            Assert.Throws<MySqlException>(() => db.Update(p));
        }
        */
        [Test]
        public void TestCreate()
        {
            CustomerProps p = new CustomerProps();
            p.Name = "Hector";
            p.Address = "101 Main Street";
            p.City = "Orlando";
            p.Customer = "FL";
            p.ZipCode = "10001";
            db.Create(p);
            CustomerProps p2 = (CustomerProps)db.Retrieve(p.CustomerID);
            Assert.AreEqual(p.GetCustomer(), p2.GetCustomer());
        }

        [Test]
        public void TestCreatePrimaryKeyViolation()
        {
            // We cant add a Customer if it already exists.
            CustomerProps p = new CustomerProps();
            p.CustomerID = 1;
            p.Name = "Molunguri, A";
            Assert.Throws<MySqlException>(() => db.Create(p));
        }
    } // end class CustomerDBTest
} // end namespace MMABooksTests
