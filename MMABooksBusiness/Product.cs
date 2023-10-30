/* Author:  Eric Robinson L00709820
  * Date:    10/29/23
 * Lane Community College CS234 Advanced Programming: C# (.NET)
 * Lab 3
 * Purpose: 
 */

using System;
using MMABooksTools;
using MMABooksProps;
using MMABooksDB;
using System.Collections.Generic;
 
namespace MMABooksBusiness
{
    public class Product : BaseBusiness  // We inherit from BaseBusiness.
    {
        
        /// <summary>
        /// Read/Write property. 
        /// </summary>
        //  Notice that I used a name for the business object property that I thought would be more intuitive for the 
        //  application programmer.  It doesn't have to match the database.
        public string ProductCode 
        {
            get
            {
                return ((ProductProps)mProps).ProductCode;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).ProductCode))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 10)
                    {
                        mRules.RuleBroken("ProductCode", false);
                        ((ProductProps)mProps).ProductCode = value;
                        mIsDirty = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("ProductCode must be no more than 10 characters long.");
                    }
                }
            }
        } // end Abbreviation

        public string Description
        {
            get
            {
                return ((ProductProps)mProps).Description;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).ProductCode))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 50)
                    {
                        mRules.RuleBroken("Description", false);
                        ((ProductProps)mProps).Description = value;
                        mIsDirty = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Description name must be no more than 50 characters long.");
                    }
                }
            }
        } // end Description

        public decimal UnitPrice
        {
            get
            {
                return ((ProductProps)mProps).UnitPrice;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).UnitPrice))
                {
                    if (value < 0)
                    {
                        mRules.RuleBroken("UnitPrice", false);
                        ((ProductProps)mProps).UnitPrice = value;
                        mIsDirty = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("UnitPrice cannot be negative.");
                    }
                }
            }
        } // end UnitPrice

        public int OnHandQuantity
        {
            get
            {
                return ((ProductProps)mProps).OnHandQuantity;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).OnHandQuantity))
                {
                    if (value < 0)
                    {
                        mRules.RuleBroken("OnHandQuantity", false);
                        ((ProductProps)mProps).OnHandQuantity = value;
                        mIsDirty = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("OnHandQuantity cannot be negative.");
                    }
                }
            }
        } // end OnHandQuantity

        public override object GetList()
        {
            List<Product> products = new List<Product>();
            List<ProductProps> props = new List<ProductProps>();

            props = (List<ProductProps>)mdbReadable.RetrieveAll();
            foreach (ProductProps prop in props)
            {
                Product p = new Product(prop.ToString());
                products.Add(p);
            }
            return products;
        } // end GetList()

        protected override void SetDefaultProperties() // ???
        {
        }

        /// <summary>
        /// Sets required fields for a record.
        /// </summary>
        protected override void SetRequiredRules() // ???
        {
            // Lists required fields.
            mRules.RuleBroken("Abbreviation", true);
            mRules.RuleBroken("Name", true);
        }

        /// <summary>
        /// Instantiates mProps and mOldProps as new Props objects. ***What is the m prefix for? Class member???
        /// Instantiates mbdReadable and mdbWriteable as new DB objects.
        /// </summary>
        protected override void SetUp()
        {
            mProps = new ProductProps();
            mOldProps = new ProductProps();
            mdbReadable = new ProductDB();
            mdbWriteable = new ProductDB();
        }

        #region constructors
        /// <summary>
        /// Default constructor - gets the connection string - assumes a new record that is not in the database.
        /// </summary>
        /// 
        // Default constructor
        public Product() : base()
        {
        }

        /// <summary>
        /// Calls methods SetUp() and Load().
        /// Use this constructor when the object is in the database AND the connection string is in a config file
        /// </summary>
        /// <param name="key">ID number of a record in the database.
        /// Sent as an arg to Load() to set values of record to properties of an 
        /// object.</param>
        public Product(string key)
            : base(key)
        {
        }

        private Product(ProductProps props)
            : base(props)
        {
        }

        #endregion

    } // end class Product : BaseBusiness
} // end namespace MMABooksBusiness
