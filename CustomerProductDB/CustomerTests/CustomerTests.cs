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

namespace CustomerTests
{
    [TestFixture]
    public class CustomerTests
    {
        private string dataSource = "Data Source=LAPTOP-834T8AA3\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";

        [SetUp]
        public void SetUpTests()
        {
            CustomerDB db = new CustomerDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void NewCustomerConstructorTest()
        {
            Customer c = new Customer(dataSource);
            Console.WriteLine(c.ToString());
            Assert.Greater(c.ToString().Length, 1);
        }

        [Test]
        public void RetrieveFromDataStoreConstructorTest()
        {
            Customer c = new Customer(1, dataSource);
            Assert.AreEqual(1, c.ID);
            Assert.AreEqual("Molunguri, A", c.Name);
            Assert.AreEqual("1108 Johanna Bay Drive", c.Address);
            Assert.AreEqual("Birmingham", c.City);
            Assert.AreEqual("AL", c.State);
            Assert.AreEqual("35216-6909", c.ZipCode);
        }

        [Test]
        public void SaveToDataStoreTest()
        {
            Customer c = new Customer(dataSource);
            c.Name = "Mouse, Mickey";
            c.Address = "123 Main Street";
            c.City = "Orlando";
            c.State = "FL";
            c.ZipCode = "10001";
            c.Save();
            Assert.AreEqual(700, c.ID);
        }

        [Test]
        public void UpdateTest()
        {
            Customer c = new Customer(dataSource);
            c.Name = "Mouse, Mickey";
            c.Address = "123 Main Street";
            c.City = "Orlando";
            c.State = "FL";
            c.ZipCode = "10001";
            c.Save();
            c = new Customer(1, dataSource);
            Assert.AreEqual(1, c.ID);
            Assert.AreEqual("Molunguri, A", c.Name);
            Assert.AreEqual("1108 Johanna Bay Drive", c.Address);
            Assert.AreEqual("Birmingham", c.City);
            Assert.AreEqual("AL", c.State);
            Assert.AreEqual("35216-6909", c.ZipCode);
        }

        [Test]
        public void DeleteTest()
        {
            Customer c = new Customer(50, dataSource);
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Customer(50, dataSource));
        }

        [Test]
        public void GetListTest()
        {
            Customer c = new Customer(dataSource);
            List<Customer> customers = (List<Customer>)c.GetList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual(157, customers[0].ID);
            Assert.AreEqual("Abeyatunge, Derek", customers[0].Name);
            Assert.AreEqual("1414 S. Dairy Ashford", customers[0].Address);
            Assert.AreEqual("North Chili", customers[0].City);
            Assert.AreEqual("NY", customers[0].State);
            Assert.AreEqual("14514", customers[0].ZipCode);
        }

        [Test]
        public void SomeRequiredPropertiesNotSetTest()
        {
            Customer c = new Customer(dataSource);
            Assert.Throws<Exception>(() => c.Save());
            c.Name = "Mouse, Mickey";
            Assert.Throws<Exception>(() => c.Save());
            c.Address = "123 Main Street";
            Assert.Throws<Exception>(() => c.Save());
            c.City = "Orlando";
            Assert.Throws<Exception>(() => c.Save());
            c.State = "FL";
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void NoRequiredPropertiesSetTest()
        {
            Customer c = new Customer(dataSource);
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void InvalidPropertyNameTest()
        {
            Customer c = new Customer(1, dataSource);
            string outOfRange = "0123456789" +
                "0123456789" +
                "0123456789" +
                "0123456789" +
                "0123456789" +
                "0123456789" +
                "0123456789" +
                "0123456789" +
                "0123456789" +
                "0123456789i";
            Assert.Throws<ArgumentOutOfRangeException>(() => c.Name = "");
            Assert.Throws<ArgumentOutOfRangeException>(() => c.Name = outOfRange);
            Assert.AreEqual(1, c.ID);
            Assert.AreEqual("Molunguri, A", c.Name);
            Assert.AreEqual("1108 Johanna Bay Drive", c.Address);
            Assert.AreEqual("Birmingham", c.City);
            Assert.AreEqual("AL", c.State);
            Assert.AreEqual("35216-6909", c.ZipCode);
        }

        [Test]
        public void InvalidPropertyAddressTest()
        {
            Customer c = new Customer(1, dataSource);
            string outOfRange = "0123456789" +
                "0123456789" +
                "0123456789" +
                "0123456789" +
                "0123456789i";
            Assert.Throws<ArgumentOutOfRangeException>(() => c.Address = "");
            Assert.Throws<ArgumentOutOfRangeException>(() => c.Address = outOfRange);
            Assert.AreEqual(1, c.ID);
            Assert.AreEqual("Molunguri, A", c.Name);
            Assert.AreEqual("1108 Johanna Bay Drive", c.Address);
            Assert.AreEqual("Birmingham", c.City);
            Assert.AreEqual("AL", c.State);
            Assert.AreEqual("35216-6909", c.ZipCode);
        }

        [Test]
        public void InvalidPropertyCityTest()
        {
            Customer c = new Customer(1, dataSource);
            string outOfRange = "0123456789" +
                "0123456789i";
            Assert.Throws<ArgumentOutOfRangeException>(() => c.City = "");
            Assert.Throws<ArgumentOutOfRangeException>(() => c.City = outOfRange);
        }

        [Test]
        public void InvalidPropertyStateTest()
        {
            Customer c = new Customer(dataSource);
            Assert.Throws<ArgumentOutOfRangeException>(() => c.State = "1");
            Assert.Throws<ArgumentOutOfRangeException>(() => c.State = "333");
        }

        [Test]
        public void InvalidPropertyZipCodeTest()
        {
            Customer c = new Customer(1, dataSource);
            string outOfRange = "0123456789" +
                "01234i";
            Assert.Throws<ArgumentOutOfRangeException>(() => c.ZipCode = "");
            Assert.Throws<ArgumentOutOfRangeException>(() => c.ZipCode = outOfRange);
            Assert.AreEqual(1, c.ID);
            Assert.AreEqual("Molunguri, A", c.Name);
            Assert.AreEqual("1108 Johanna Bay Drive", c.Address);
            Assert.AreEqual("Birmingham", c.City);
            Assert.AreEqual("AL", c.State);
            Assert.AreEqual("35216-6909", c.ZipCode);
        }

        [Test]
        public void ConcurrencyIssueTest()
        {
            Customer c1 = new Customer(1, dataSource);
            Customer c2 = new Customer(1, dataSource);

            c1.Name = "Mouse, Mickey";
            c1.Save();
            c2.Name = "Mouse, Minne";

            Assert.Throws<Exception>(() => c2.Save());
        }
    }
}
