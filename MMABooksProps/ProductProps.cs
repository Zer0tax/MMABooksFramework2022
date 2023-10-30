/* Author:  Eric Robinson L00709820
  * Date:    10/25/23
 * Lane Community College CS234 Advanced Programming: C# (.NET)
 * Lab 3
 * Code adapted from CustomerProps.cs
 * Purpose: 
 */

using System;
using MMABooksTools;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader; // Using an alias here allows us to more easily change the DBMS.
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MMABooksProps
{
    [Serializable()] // What does this mean?
    public class ProductProps : IBaseProps  // Inherits IBaseProps. 
    {
        #region Auto-implemented Properties
        public string ProductCode { get; set; } = "";

        public string Description { get; set; } = "";

        public decimal UnitPrice { get; set; } = 0m;

        public int OnHandQuantity { get; set; } = 0;

        /// <summary>
        /// ConcurrencyID. Don't manipulate directly.
        /// </summary>
        public int ConcurrencyID { get; set; } = 0;
        #endregion

        // Clone() allows us to make copies of our object.
        public object Clone()
        // We need to have this method because our class, CustomerProps, inherits from the IBaseProps interface and
        // IBaseProps is marked as ICloneable in IBaseProps.cs.
        {
            ProductProps p = new ProductProps();
            p.ProductCode = this.ProductCode;
            p.Description = this.Description;
            p.UnitPrice = this.UnitPrice;
            p.OnHandQuantity = this.OnHandQuantity;
            p.ConcurrencyID = this.ConcurrencyID;
            return p;
        }

        // LS: This is always the same ... so I should have made IBaseProps an abstract class.
        public string GetCustomer()
        {
            string jsonString;
            jsonString = JsonSerializer.Serialize(this);
            return jsonString;
        }

        public string GetState()
        {
            string jsonString;
            jsonString = JsonSerializer.Serialize(this);
            return jsonString;
        }

        public void SetCustomer(string jsonString)
        {
            ProductProps p = JsonSerializer.Deserialize<ProductProps>(jsonString); // What does this do???
            this.ProductCode = p.ProductCode;
            this.Description = p.Description;
            this.UnitPrice = p.UnitPrice;   
            this.OnHandQuantity = p.OnHandQuantity;
            this.ConcurrencyID = p.ConcurrencyID;
        }

        public void SetCustomer(DBDataReader dr)
        {
            this.ProductCode = ((string)dr["ProductCode"]).Trim();
            this.Description = ((string)dr["Description"]).Trim();
            this.UnitPrice = (decimal)dr["UnitPrice"];
            this.OnHandQuantity = (int)dr["OnHandQuantity"];
            this.ConcurrencyID = (Int32)dr["ConcurrencyID"];
        }

        public void SetState(string jsonString)
        {
            ProductProps p = JsonSerializer.Deserialize<ProductProps>(jsonString); // What does this do???
            this.ProductCode = p.ProductCode;
            this.Description = p.Description;
            this.UnitPrice = p.UnitPrice;
            this.OnHandQuantity= p.OnHandQuantity;
            this.ConcurrencyID = p.ConcurrencyID;
        }

        public void SetState(DBDataReader dr)
        {
            throw new NotImplementedException();
        }
    } // end class ProductProps : IBaseProps
} // end namespace MMABooksProps
