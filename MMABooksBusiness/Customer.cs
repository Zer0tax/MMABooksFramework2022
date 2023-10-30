/* Author:  Lindy Stewart
 * Changes: Eric Robinson L00709820
 * Date:    10/29/23
 * Lane Community College CS234 Advanced Programming: C# (.NET)
 * Lab 3
 * Purpose: 
 */

using MMABooksTools;
using MMABooksProps;
using MMABooksDB;
using System;
using System.Collections.Generic;
using System.Text;
 
namespace MMABooksBusiness
{
    public class Customer : BaseBusiness  // We inherit from BaseBusiness.
    {
        public int CustomerID
        {
            get
            {
                return ((CustomerProps)mProps).CustomerID;
            }
            // There is no setter for CustomerID. It is auto-assigned.
        }

        public string Name
        {
            get
            {
                return ((CustomerProps)mProps).Name;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).Name))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 100)
                    {
                        mRules.RuleBroken("Name", false);
                        ((CustomerProps)mProps).Name = value;
                        mIsDirty = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Name must be no more than 100 characters long.");
                    }
                }
            }
        } // end Name

        public string Address
        {
            get
            {
                return ((CustomerProps)mProps).Address;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).Address))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 50)
                    {
                        mRules.RuleBroken("Address", false);
                        ((CustomerProps)mProps).Address = value;
                        mIsDirty = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Address must be no more than 50 characters long.");
                    }
                }
            }
        } // end  Address

        public string City
        {
            get
            {
                return ((CustomerProps)mProps).City;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).City))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 20)
                    {
                        mRules.RuleBroken("City", false);
                        ((CustomerProps)mProps).City = value;
                        mIsDirty = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("City must be no more than 20 characters long.");
                    }
                }
            }
        } // end City

        public string State
        {
            get
            {
                return ((CustomerProps)mProps).Customer;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).Customer))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 2)
                    {
                        mRules.RuleBroken("Customer", false);
                        ((CustomerProps)mProps).Customer = value;
                        mIsDirty = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Customer must be no more than 2 characters long.");
                    }
                }
            }
        } // end Customer

        public string ZipCode
        {
            get
            {
                return ((CustomerProps)mProps).ZipCode;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).ZipCode))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 15)
                    {
                        mRules.RuleBroken("ZipCode", false);
                        ((CustomerProps)mProps).ZipCode = value;
                        mIsDirty = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("ZipCode must be no more than 15 characters long.");
                    }
                }
            }
        } // end ZipCode

        protected override void SetRequiredRules()
        {
            // Lists required fields.
            // We dont need to list CustomerID?
            mRules.RuleBroken("Name", true);
            mRules.RuleBroken("Address", true);
            mRules.RuleBroken("City", true);
            mRules.RuleBroken("Customer", true);
            mRules.RuleBroken("ZipCode", true);
        }

        protected override void SetDefaultProperties()
        {
            // We need this because it is required by our interface.
            // But we dont need to actually do anything in this class.
        }

        /// <summary>
        /// Instantiates mProps and mOldProps as new Props objects. ***What is the m prefix for? Class member???
        /// Instantiates mbdReadable and mdbWriteable as new DB objects.
        /// </summary>
        protected override void SetUp()
        {
            mProps = new CustomerProps();
            mOldProps = new CustomerProps();
            mdbReadable = new CustomerDB();
            mdbWriteable = new CustomerDB();
        }

        public override object GetList()
        {
            List<Customer> customers = new List<Customer>();
            List<CustomerProps> props = new List<CustomerProps>();

            props = (List<CustomerProps>)mdbReadable.RetrieveAll();
            foreach (CustomerProps prop in props)
            {
                Customer c = new Customer(prop);
                customers.Add(c);
            }
            return customers;
        }

        #region constructors
        /// <summary>
        /// Default constructor - gets the connection string - 
        /// assumes a new record that is not in the database.
        /// </summary>
        /// 
        // Default constructor
        public Customer() : base()
        {
        }

        /// <summary>
        /// Calls methods SetUp() and Load().
        /// Use this constructor when the object is in the database AND the connection string is in a config file
        /// </summary>
        /// <param name="key">ID number of a record in the database.
        /// Sent as an arg to Load() to set values of record to properties of an 
        /// object.</param>
        public Customer(int key)
            : base(key)
        {
        }

        private Customer(CustomerProps props)
            : base(props)
        {
        }

        #endregion
    } // end class Customer
} // end namespace MMABooksBusiness
