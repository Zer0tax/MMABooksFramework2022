/* Author:  Eric Robinson L00709820
  * Date:    10/29/23
 * Lane Community College CS234 Advanced Programming: C# (.NET)
 * Lab 3
 * Code copied from CustomerTests.cs
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
    internal class ProductTests
    {
        [SetUp]

        public void TestResetDatabase()
        // This method runs before every test.
        {
            ProductDB db = new ProductDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetProductData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestNewCProductConstructor()
        {
            Product p = new Product();
            Assert.AreEqual(string.Empty, p.ProductCode);
            Assert.AreEqual(string.Empty, p.Description);
            Assert.IsTrue(p.IsNew);
            Assert.IsFalse(p.IsValid);
        }

        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // Retrieves from Data Store.
            // I am trying to avoid comparing to any field that contains an apostrophe.
            Product p = new Product("CS10");
            Assert.AreEqual(56.5, p.UnitPrice);
            Assert.AreEqual(5136, p.OnHandQuantity);
            Assert.IsFalse(p.IsNew);
            Assert.IsTrue(p.IsValid);
        }

        [Test]
        public void TestSaveToDataStore()  
        {
            // This SHOULD create a new product.
            // We want to make sure that it gets saved to the DB.
            // The business object (?) has a Save() method that is
            // smart enought to know if it should do an
            // Insert, Update or Delete(?) depending on the Customer of the  object. 
            Product p = new Product();
            p.ProductCode = "DEFG";
            p.Description = "Another Book by Eric";
            p.UnitPrice = 25.25m;
            p.OnHandQuantity = 11;
            p.Save();
            Product p2 = new Product(p.ProductCode); 
            Assert.AreEqual(p2.ProductCode, p.ProductCode);
            Assert.AreEqual(p2.Description, p2.Description);
        }

        [Test]
        public void TestUpdate()
        {
            Product p1 = new Product();
            p1.ProductCode = "HIJK";
            p1.Description = "The Sound and the Fury";
            p1.UnitPrice = 12.15m;
            p1.OnHandQuantity = 33;
            p1.Save();

            Product p2 = new Product();
            p2.ProductCode = "HIJK";
            p2.Description = "The Sound and the Fury";
            p2.UnitPrice = 12.15m;
            p2.OnHandQuantity = 33;
            p2.Save();
            Assert.AreEqual(p1.ProductCode, p2.ProductCode);
            Assert.AreEqual(p2.Description, p2.Description);
        }

        [Test]
        public void TestDelete()
        {
            Product p = new Product();
            p.Delete();
            p.Save();
            // I really have no idea how the next line works.
            Assert.Throws<Exception>(() => new Product());
        }

        [Test]
        public void TestGetList()
        {
            Product p = new Product();
            List<Product> products = (List<Product>)p.GetList();
            Assert.AreEqual("A4CS", products[1].ProductCode);
            Assert.AreEqual("Murach''s ASP.NET 4 Web Programming with C# 2010", products[1].Description);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Product p = new Product();
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            // Another thest that I don't understand.
            Product p = new Product();
            Assert.Throws<Exception>(() => p.Save());
            p.Description = "??";
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestInvalidPropertySet()
        {
            Product p = new Product();
            Assert.Throws<ArgumentOutOfRangeException>(() => p.ProductCode = "12345678901"); // Product code must be <= 10 chars. Test 11,
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            Product p1 = new Product("HIJK");
            Product p2 = new Product("HIJK");

            p1.Description = "Updated 1st";
            p1.UnitPrice = 12.15m;
            p1.OnHandQuantity = 33;
            p1.Save();

            p2.Description = "Updated 2nd";
            p2.UnitPrice = 22.15m;
            p2.OnHandQuantity = 66;
            p2.Save();

            Assert.Throws<Exception>(() => p2.Save()); // Huh???
        }

    } // end class ProductTests
} // end namespace MMABooksTests
