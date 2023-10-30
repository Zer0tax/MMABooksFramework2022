/* Author:  Lindy Stewart
 * Changes: Eric Robinson L00709820
 * Date:    10/25/23
 * Lane Community College CS234 Advanced Programming: C# (.NET)
 * Lab 3
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
    public class StateProps : IBaseProps  // Inherits IBaseProps. 
    {
        #region Auto-implemented Properties
        public string Code { get; set; } = "";

        public string Name { get; set; } = "";

        /// <summary>
        /// ConcurrencyID. Don't manipulate directly.
        /// </summary>
        public int ConcurrencyID { get; set; } = 0;
        #endregion

        // Clone() allows us to make copies of our object.
        public object Clone() 
        // We need to have this method because our class, StateProps, inherits from the IBaseProps interface and
        // IBaseProps is marked as ICloneable in IBaseProps.cs.
        {
            StateProps p = new StateProps();
            p.Code = this.Code;
            p.Name = this.Name;
            p.ConcurrencyID = this.ConcurrencyID;
            return p;
        }

        // LS: This is always the same ... so I should have made IBaseProps an abstract class.
        public string GetState()
        {
            string jsonString;
            jsonString = JsonSerializer.Serialize(this);
            return jsonString;
        }

        public void SetState(string jsonString)
        {
            StateProps p = JsonSerializer.Deserialize<StateProps>(jsonString); // What does this do???
            this.Code = p.Code;
            this.Name = p.Name;
            this.ConcurrencyID = p.ConcurrencyID;
        }

        public void SetState(DBDataReader dr)
        {
            this.Code = ((string)dr["StateCode"]).Trim();
            this.Name = (string)dr["StateName"];
            this.ConcurrencyID = (Int32)dr["ConcurrencyID"];
        }

    } // end class StateProps : IBaseProps
} // end namespace MMABooksProps
