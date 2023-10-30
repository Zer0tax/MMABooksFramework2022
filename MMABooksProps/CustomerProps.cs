/* Author:  Lindy Stewart
 * Changes: Eric Robinson L00709820
 * Date:    10/25/23
 * Lane Community College CS234 Advanced Programming: C# (.NET)
 * Lab 3
 * Purpose: 
 * Notes: See Video 4 for instructions on how to create these methods.
 */

using System;
using System.Collections.Generic;
using System.Text;

using MMABooksTools;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader; // Using an alias here allows us to more easily change the DBMS.
using System.Text.Json; // We use this to parse JSON strings (supposedly)
using System.Text.Json.Serialization;

namespace MMABooksProps
{

    /* CUSTOMERPROPS–CONTAINS PROPERTIES FOR EACH OF THE FIELDS IN THE DATABASE.
     * IT IMPLEMENTS ICLONEABLE AND IBASEPROPS.
     * IT MUST CONTAIN AN IMPLEMENTATION OF CLONE, GETCustomer AND SETCustomer.
     * We use the Customer class as our model for reference.
     * */

    public class CustomerProps : IBaseProps  // Inherits IBaseProps. 
    {

        // Get/Set properties for Customer.
        #region Auto-implemented Properties
        public int CustomerID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string Customer { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; }
        public string ZipCode { get; set; } = "";
        // Why dont we need a setter for ConcurrencyID???

        /// <summary>
        /// ConcurrencyID. Don't manipulate directly. But what is it for???
        /// </summary>
        public int ConcurrencyID { get; set; } = 0;
        #endregion

        public object Clone()
        {
            // We need to have this method because our class, CustomerProps, inherits from the IBaseProps interface and
            // IBaseProps is marked as ICloneable in IBaseProps.cs.
            {
                CustomerProps p = new CustomerProps();
                p.CustomerID = this.CustomerID;
                p.Name = this.Name;
                p.Address = this.Address;
                p.Customer = this.Customer;
                p.City = this.City;
                p.State = this.State;
                p.ZipCode = this.ZipCode;
                p.ConcurrencyID = this.ConcurrencyID;
                return p;
            }
        }

        public string GetCustomer()
        // Not even the tiniest, faintest clue what this does.
        {
            string jsonString;
            jsonString = JsonSerializer.Serialize(this);
            return jsonString;
        }

        public string GetState()
        {
            throw new NotImplementedException();
        }

        /*       public string GetCustomer()
             {
                 // Why are we using a serializer here???
                 string jsonString;
                 jsonString = JsonSerializer.Serialize(this);
                 return jsonString;
             }
      */

        public void SetCustomer(string jsonString)
        // Why do we have this???
        {
            CustomerProps p = JsonSerializer.Deserialize<CustomerProps>(jsonString); // What does this do???
            this.CustomerID = p.CustomerID;
            this.Name = p.Name;
            this.Address = p.Address;
            this.Customer = p.Customer;
            this.City = p.City;
            this.ZipCode = p.ZipCode;
            this.ConcurrencyID = p.ConcurrencyID;
        }

        public void SetCustomer(DBDataReader dr)
        {
            this.CustomerID = ((int)dr["CustomerID"]);
            this.Name = (string)dr["Name"];
            this.Address = (string)dr["Address"];
            this.City = (string)dr["City"];
            this.Customer = (string)dr["Customer"];
            this.ZipCode = (string)dr["ZipCode"];
            this.ConcurrencyID = (Int32)dr["ConcurrencyID"]; 
        }

/*        public void SetCustomer(string jsonString)
        {
            // Why are we using a serializer here???
            CustomerProps p = JsonSerializer.Deserialize<CustomerProps>(jsonString); // What does this do???
            this.CustomerID = p.CustomerID;
            this.Name = p.Name;
            this.Address = p.Address;
            this.City = p.City;
            this.Customer = p.Customer;
            this.ZipCode = p.ZipCode;
            this.ConcurrencyID = p.ConcurrencyID;
        }

        public void SetCustomer(DBDataReader dr)
        {
            this.CustomerID = (int)dr["CustomerID"];
            this.Name = (string)dr["Name"];
            this.Address = (string)dr["Address"];
            this.City = (string)dr["City"];
            this.Customer = (string)dr["Customer"];
            this.ZipCode = (string)dr["ZipCode"];

            this.ConcurrencyID = (Int32)dr["ConcurrencyID"];
        }
*/
        public void SetState(string jsonString)
        {
            throw new NotImplementedException();
        }

        public void SetState(DBDataReader dr)
        {
            throw new NotImplementedException();
        }
    } // end class CustomerProps
} // end namespace MMABooksProps
