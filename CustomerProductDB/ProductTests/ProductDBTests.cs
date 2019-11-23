using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CustomerProductPropsClasses;
using CustomerProductDBClasses;

namespace ProductTests
{
    [TestFixture]
    public class ProductDBTests
    {
        ProductDB db;
        private string dataSource = "Data Source=LAPTOP-834T8AA3\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";
        ProductProps newProps;

        [SetUp]
        public void SetUpTests()
        {
            db = new ProductDB(dataSource);
            newProps = new ProductProps
            {
                productCode = "ABC123",
                description = "Awesome Sauce!",
                unitPrice = 100m,
                onHandQuantity = 12,
            };
        }

        [Test]
        public void RetriveTest()
        {
            ProductProps props = (ProductProps)db.Retrieve(1);
            Assert.AreEqual(1, props.ID);
            Assert.AreEqual("A4CS", props.productCode);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", props.description);
            Assert.AreEqual(56.50m, props.unitPrice);
            Assert.AreEqual(4637, props.onHandQuantity);
            Assert.AreEqual(1, props.ConcurrencyID);
        }

        [Test]
        public void RetriveInvalidKeyTest()
        {
            Assert.Throws<Exception>(() => db.Retrieve(50));
        }

        [Test]
        public void RetriveAllTest()
        {
            List<ProductProps> propsList = (List<ProductProps>)db.RetrieveAll(db.GetType());
            Assert.AreEqual(16, propsList.Count);
        }

        [Test]
        public void CreateTest()
        {
            ProductProps props = (ProductProps)db.Create(newProps);
            Assert.AreEqual(1, props.ConcurrencyID);
            int id = props.ID;
            props = (ProductProps)db.Retrieve(id);
            Assert.AreEqual("ABC123", props.productCode);
            Assert.AreEqual("Awesome Sauce!", props.description);
            Assert.AreEqual(100m, props.unitPrice);
            Assert.AreEqual(12, props.onHandQuantity);
            db.Delete(props);
        }

        [Test]
        public void DeleteTest()
        {
            ProductProps props = (ProductProps)db.Create(newProps);
            int id = props.ID;
            Assert.IsTrue(db.Delete(props));
            Assert.Throws<Exception>(() => db.Retrieve(id));
            Assert.Throws<Exception>(() => db.Delete(props));
        }

        [Test]
        public void DeleteInvalidConcurrencyIDTest()
        {
            ProductProps props = (ProductProps)db.Create(newProps);
            props.ConcurrencyID++;
            Assert.Throws<Exception>(() => db.Delete(props));
            props.ConcurrencyID--;
            db.Delete(props);
        }

        [Test]
        public void UpdateTest()
        {
            ProductProps props = (ProductProps)db.Create(newProps);
            props.productCode = "XYZ987";
            props.description = "Average Sauce.";
            props.unitPrice = 2m;
            props.onHandQuantity= 97214;
            int id = props.ID;
            Assert.IsTrue(db.Update(props));
            props = (ProductProps)db.Retrieve(id);
            Assert.AreEqual(id, props.ID);
            Assert.AreEqual("XYZ987", props.productCode);
            Assert.AreEqual("Average Sauce.", props.description);
            Assert.AreEqual(2m, props.unitPrice);
            Assert.AreEqual(97214, props.onHandQuantity);
            Assert.AreEqual(2, props.ConcurrencyID);
            db.Delete(props);
        }

        [Test]
        public void UpdateInvalidConcurrencyIDTest()
        {
            ProductProps props = (ProductProps)db.Create(newProps);
            props.productCode = "XYZ987";
            props.description = "Average Sauce.";
            props.unitPrice = 2m;
            props.onHandQuantity = 97214;
            int id = props.ID;
            props.ConcurrencyID++;
            Assert.Throws<Exception>(() => db.Update(props));
            props.ConcurrencyID--;
            db.Delete(props);
        }
    }
}
