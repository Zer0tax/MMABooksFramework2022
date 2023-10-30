using NUnit.Framework;

using MMABooksProps;
using System;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductPropsTests
    {
        ProductProps props;
        [SetUp]
        public void Setup()
        {
            props = new ProductProps();
            props.ProductCode = "987";
            props.Description = "This is a test";
            props.UnitPrice = 1.0m;
            props.OnHandQuantity = 1;
        }

        [Test]
        public void TestGetProduct()
        {
            string jsonString = props.GetState();
            Console.WriteLine(jsonString);
            Assert.IsTrue(jsonString.Contains(props.ProductCode));
            Assert.IsTrue(jsonString.Contains(props.Description));
            Assert.IsTrue(jsonString.Contains((props.UnitPrice).ToString()));
            Assert.IsTrue(jsonString.Contains((props.OnHandQuantity).ToString()));
        }

        [Test]
        public void TestSetProduct()
        {
            string jsonString = props.GetState();
            ProductProps newProps = new ProductProps();
            newProps.SetState(jsonString);
            Assert.AreEqual(props.ProductCode, newProps.ProductCode);
            Assert.AreEqual(props.Description, newProps.Description);
            Assert.AreEqual(props.UnitPrice, newProps.UnitPrice);
            Assert.AreEqual(props.OnHandQuantity, newProps.OnHandQuantity);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }

        [Test]
        public void TestClone()
        {
            ProductProps newProps = (ProductProps)props.Clone();
            Assert.AreEqual(props.ProductCode, newProps.ProductCode);
            Assert.AreEqual(props.Description, newProps.Description);
            Assert.AreEqual(props.UnitPrice, newProps.UnitPrice);
            Assert.AreEqual(props.OnHandQuantity, newProps.OnHandQuantity);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }
    }
}