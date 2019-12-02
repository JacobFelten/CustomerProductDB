using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CustomerProductPropsClasses;
using CustomerProductDBClasses;
using CustomerProductClasses;

using System.Data;
using System.Data.SqlClient;

using DBCommand = System.Data.SqlClient.SqlCommand;

namespace ProductTests
{
    [TestFixture]
    public class ProductTests
    {
        private string dataSource = "Data Source=LAPTOP-834T8AA3\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";

        [SetUp]
        public void SetUpTests()
        {
            ProductDB db = new ProductDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void NewProductConstructorTest()
        {
            Product p = new Product(dataSource);
            Console.WriteLine(p.ToString());
            Assert.Greater(p.ToString().Length, 1);
        }

        [Test]
        public void RetrieveFromDataStoreConstructorTest()
        {
            Product p = new Product(1, dataSource);
            Assert.AreEqual(1, p.ID);
            Assert.AreEqual("A4CS", p.ProductCode);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", p.Description);
            Assert.AreEqual(56.50m, p.UnitPrice);
            Assert.AreEqual(4637, p.OnHandQuantity);
        }

        [Test]
        public void SaveToDataStoreTest()
        {
            Product p = new Product(dataSource);
            p.ProductCode = "ABC123";
            p.Description = "Awesome Sauce!";
            p.UnitPrice = 100m;
            p.OnHandQuantity = 12;
            p.Save();
            Assert.AreEqual(17, p.ID);
        }

        [Test]
        public void UpdateTest()
        {
            Product p = new Product(dataSource);
            p.ProductCode = "ABC123";
            p.Description = "Awesome Sauce!";
            p.UnitPrice = 100m;
            p.OnHandQuantity = 12;
            p.Save();
            p = new Product(1, dataSource);
            Assert.AreEqual(1, p.ID);
            Assert.AreEqual("A4CS", p.ProductCode);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", p.Description);
            Assert.AreEqual(56.50m, p.UnitPrice);
            Assert.AreEqual(4637, p.OnHandQuantity);
        }

        [Test]
        public void DeleteTest()
        {
            Product p = new Product(5, dataSource);
            p.Delete();
            p.Save();
            Assert.Throws<Exception>(() => new Product(5, dataSource));
        }

        [Test]
        public void GetListTest()
        {
            Product p = new Product(dataSource);
            List<Product> products = (List<Product>)p.GetList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual(7, products[0].ID);
            Assert.AreEqual("DB1R", products[0].ProductCode);
            Assert.AreEqual("DB2 for the COBOL Programmer, Part 1 (2nd Edition)", products[0].Description);
            Assert.AreEqual(42m, products[0].UnitPrice);
            Assert.AreEqual(4825, products[0].OnHandQuantity);
        }

        [Test]
        public void SomeRequiredPropertiesNotSetTest()
        {
            Product p = new Product(dataSource);
            Assert.Throws<Exception>(() => p.Save());
            p.ProductCode = "ABC123";
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void NoRequiredPropertiesSetTest()
        {
            Customer c = new Customer(dataSource);
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void InvalidPropertyProductCodeTest()
        {
            Product p = new Product(1, dataSource);
            string outOfRange = "0123456789i";
            Assert.Throws<ArgumentOutOfRangeException>(() => p.ProductCode = "");
            Assert.Throws<ArgumentOutOfRangeException>(() => p.ProductCode = outOfRange);
            Assert.AreEqual(1, p.ID);
            Assert.AreEqual("A4CS", p.ProductCode);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", p.Description);
            Assert.AreEqual(56.50m, p.UnitPrice);
            Assert.AreEqual(4637, p.OnHandQuantity);
        }

        [Test]
        public void InvalidPropertyDescriptionTest()
        {
            Product p = new Product(1, dataSource);
            string outOfRange = "0123456789" +
                "0123456789" +
                "0123456789" +
                "0123456789" +
                "0123456789i";
            Assert.Throws<ArgumentOutOfRangeException>(() => p.Description = "");
            Assert.Throws<ArgumentOutOfRangeException>(() => p.Description = outOfRange);
            Assert.AreEqual(1, p.ID);
            Assert.AreEqual("A4CS", p.ProductCode);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", p.Description);
            Assert.AreEqual(56.50m, p.UnitPrice);
            Assert.AreEqual(4637, p.OnHandQuantity);
        }      

        [Test]
        public void ConcurrencyIssueTest()
        {
            Product p1 = new Product(1, dataSource);
            Product p2 = new Product(1, dataSource);

            p1.ProductCode = "ABC123";
            p1.Save();
            p2.ProductCode = "XYZ987";

            Assert.Throws<Exception>(() => p2.Save());
        }
    }
}
