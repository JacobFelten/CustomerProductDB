using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CustomerProductPropsClasses;
using CustomerProductDBClasses;

namespace CustomerTests
{
    [TestFixture]
    public class CustomerDBTests
    {
        CustomerDB db;
        private string dataSource = "Data Source=LAPTOP-834T8AA3\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";
        CustomerProps newProps;

        [SetUp]
        public void SetUpTests()
        {
            db = new CustomerDB(dataSource);
            newProps = new CustomerProps
            {
                name = "Mouse, Mickey",
                address = "123 Main Street",
                city = "Orlando",
                state = "FL",
                zipCode = "10001",
            };
        }
        
        [Test]
        public void RetriveTest()
        {
            CustomerProps props = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual(1, props.ID);
            Assert.AreEqual("Molunguri, A", props.name);
            Assert.AreEqual("1108 Johanna Bay Drive", props.address);
            Assert.AreEqual("Birmingham", props.city);
            Assert.AreEqual("AL", props.state);
            Assert.AreEqual("35216-6909", props.zipCode);
            Assert.AreEqual(1, props.ConcurrencyID);
        }

        [Test]
        public void RetriveInvalidKeyTest()
        {
            Assert.Throws<Exception>(() => db.Retrieve(750));
        }

        [Test]
        public void RetriveAllTest()
        {
            List<CustomerProps> propsList = (List<CustomerProps>)db.RetrieveAll(db.GetType());
            Assert.AreEqual(699, propsList.Count);
        }

        [Test]
        public void CreateTest()
        { 
            CustomerProps props = (CustomerProps)db.Create(newProps);
            Assert.AreEqual(1, props.ConcurrencyID);
            int id = props.ID;
            props = (CustomerProps)db.Retrieve(id);
            Assert.AreEqual("Mouse, Mickey", props.name);
            Assert.AreEqual("123 Main Street", props.address);
            Assert.AreEqual("Orlando", props.city);
            Assert.AreEqual("FL", props.state);
            Assert.AreEqual("10001", props.zipCode);
            db.Delete(props);
        }

        [Test]
        public void DeleteTest()
        {
            CustomerProps props = (CustomerProps)db.Create(newProps);
            int id = props.ID;
            Assert.IsTrue(db.Delete(props));
            Assert.Throws<Exception>(() => db.Retrieve(id));
            Assert.Throws<Exception>(() => db.Delete(props));
        }

        [Test]
        public void DeleteInvalidConcurrencyIDTest()
        {
            CustomerProps props = (CustomerProps)db.Create(newProps);
            props.ConcurrencyID++;
            Assert.Throws<Exception>(() => db.Delete(props));
            props.ConcurrencyID--;
            db.Delete(props);
        }

        [Test]
        public void UpdateTest()
        {
            CustomerProps props = (CustomerProps)db.Create(newProps);
            props.name = "Mouse, Minne";
            props.address = "987 First Street";
            props.city = "Anaheim";
            props.state = "CA";
            props.zipCode = "99999";
            int id = props.ID;
            Assert.IsTrue(db.Update(props));
            props = (CustomerProps)db.Retrieve(id);
            Assert.AreEqual(id, props.ID);
            Assert.AreEqual("Mouse, Minne", props.name);
            Assert.AreEqual("987 First Street", props.address);
            Assert.AreEqual("Anaheim", props.city);
            Assert.AreEqual("CA", props.state);
            Assert.AreEqual("99999", props.zipCode);
            Assert.AreEqual(2, props.ConcurrencyID);
            db.Delete(props);
        }

        [Test]
        public void UpdateInvalidConcurrencyIDTest()
        {
            CustomerProps props = (CustomerProps)db.Create(newProps);
            props.name = "Mouse, Minne";
            props.address = "987 First Street";
            props.city = "Anaheim";
            props.state = "CA";
            props.zipCode = "99999";
            int id = props.ID;
            props.ConcurrencyID++;
            Assert.Throws<Exception>(() => db.Update(props));
            props.ConcurrencyID--;
            db.Delete(props);
        }
    }
}
