using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CustomerProductPropsClasses;
using ToolsCSharp;
using CustomerProductDBClasses;

using System.Data;

namespace CustomerProductClasses
{
    public class Product : BaseBusiness
    {
        #region SetUpStuff
        /// <summary>
        /// 
        /// </summary>		
        protected override void SetDefaultProperties()
        {
        }

        /// <summary>
        /// Sets required fields for a record.
        /// </summary>
        protected override void SetRequiredRules()
        {
            mRules.RuleBroken("ProductCode", true);
            mRules.RuleBroken("Description", true);
        }

        /// <summary>
        /// Instantiates mProps and mOldProps as new Props objects.
        /// Instantiates mbdReadable and mdbWriteable as new DB objects.
        /// </summary>
        protected override void SetUp()
        {
            mProps = new ProductProps();
            mOldProps = new ProductProps();

            if (this.mConnectionString == "")
            {
                mdbReadable = new ProductDB();
                mdbWriteable = new ProductDB();
            }

            else
            {
                mdbReadable = new ProductDB(this.mConnectionString);
                mdbWriteable = new ProductDB(this.mConnectionString);
            }
        }
        #endregion

        #region constructors
        /// <summary>
        /// Default constructor - does nothing.
        /// </summary>
        public Product() : base()
        {
        }

        /// <summary>
        /// One arg constructor.
        /// Calls methods SetUp(), SetRequiredRules(), 
        /// SetDefaultProperties() and BaseBusiness one arg constructor.
        /// </summary>
        /// <param name="cnString">DB connection string.
        /// This value is passed to the one arg BaseBusiness constructor, 
        /// which assigns the it to the protected member mConnectionString.</param>
        public Product(string cnString)
            : base(cnString)
        {
        }

        /// <summary>
        /// Two arg constructor.
        /// Calls methods SetUp() and Load().
        /// </summary>
        /// <param name="key">ID number of a record in the database.
        /// Sent as an arg to Load() to set values of record to properties of an 
        /// object.</param>
        /// <param name="cnString">DB connection string.
        /// This value is passed to the one arg BaseBusiness constructor, 
        /// which assigns the it to the protected member mConnectionString.</param>
        public Product(int key, string cnString)
            : base(key, cnString)
        {
        }

        public Product(int key)
            : base(key)
        {
        }

        // *** I added these 2 so that I could create a 
        // business object from a properties object
        // I added the new constructors to the base class
        public Product(ProductProps props)
            : base(props)
        {
        }

        public Product(ProductProps props, string cnString)
            : base(props, cnString)
        {
        }
        #endregion

        #region properties
        /// <summary>
        /// Read-only ID property.
        /// </summary>
        public int ID
        {
            get
            {
                return ((ProductProps)mProps).ID;
            }
        }

        /// <summary>
        /// Read/Write property. 
        /// </summary>
        public string ProductCode
        {
            get
            {
                return ((ProductProps)mProps).productCode;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).productCode))
                {
                    if (value != "" && value.Length <= 10)
                    {
                        mRules.RuleBroken("ProductCode", false);
                        ((ProductProps)mProps).productCode = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("ProductCode must be between 1 and 10 characters");
                    }
                }
            }
        }

        /// <summary>
        /// Read/Write property. 
        /// </summary>
        /// <exception cref="ArgumentException">
        /// 
        /// </exception>
        public string Description
        {
            get
            {
                return ((ProductProps)mProps).description;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).description))
                {
                    if (value != "" && value.Length <= 50)
                    {
                        mRules.RuleBroken("Description", false);
                        ((ProductProps)mProps).description = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("Description must be between 1 and 50 characters");
                    }
                }
            }
        }

        /// <summary>
        /// Read/Write property. 
        /// </summary>
        /// <exception cref="ArgumentException">
        /// 
        /// </exception>
        public decimal UnitPrice
        {
            get
            {
                return ((ProductProps)mProps).unitPrice;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).unitPrice))
                {
                    ((ProductProps)mProps).unitPrice = value;
                    mIsDirty = true;
                }
            }
        }

        /// <summary>
        /// Read/Write property. 
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown if the value is null or less than 1.
        /// </exception>
        public int OnHandQuantity
        {
            get
            {
                return ((ProductProps)mProps).onHandQuantity;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).onHandQuantity))
                {
                    ((ProductProps)mProps).onHandQuantity = value;
                    mIsDirty = true;
                }
            }
        }
        #endregion

        public override object GetList()
        {
            List<Product> products = new List<Product>();
            List<ProductProps> props = new List<ProductProps>();


            props = (List<ProductProps>)mdbReadable.RetrieveAll(props.GetType());
            foreach (ProductProps prop in props)
            {
                Product p = new Product(prop, this.mConnectionString);
                products.Add(p);
            }

            return products;
        }
    }
}
