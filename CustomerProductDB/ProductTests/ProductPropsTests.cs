using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CustomerProductPropsClasses;

namespace ProductTests
{
    [TestFixture]
    public class ProductPropsTests
    {
        ProductProps props;

        [SetUp]
        public void SetUpTests()
        {
            props = new ProductProps();
            props.ID = 1;
            props.productCode = "A123";
            props.description = "Quality Product";
            props.unitPrice = 9.99m;
            props.onHandQuantity = 12;
            props.ConcurrencyID = 4;
        }

        [Test]
        public void GetStateTest()
        {
            string output = props.GetState();
            Console.WriteLine(output);
        }

        [Test]
        public void SetStateTest()
        {
            ProductProps props2 = new ProductProps();
            props2.SetState(props.GetState());
            Assert.AreEqual(props.ID, props2.ID);
            Assert.AreEqual(props.productCode, props2.productCode);
            Assert.AreEqual(props.description, props2.description);
            Assert.AreEqual(props.unitPrice, props2.unitPrice);
            Assert.AreEqual(props.onHandQuantity, props2.onHandQuantity);
            Assert.AreEqual(props.ConcurrencyID, props2.ConcurrencyID);
        }

        [Test]
        public void CloneTest()
        {
            ProductProps props2;
            props2 = (ProductProps)props.Clone();
            Assert.AreEqual(props.ID, props2.ID);
            Assert.AreEqual(props.productCode, props2.productCode);
            Assert.AreEqual(props.description, props2.description);
            Assert.AreEqual(props.unitPrice, props2.unitPrice);
            Assert.AreEqual(props.onHandQuantity, props2.onHandQuantity);
            Assert.AreEqual(props.ConcurrencyID, props2.ConcurrencyID);
        }
    }
}
