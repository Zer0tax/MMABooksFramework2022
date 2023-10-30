/* Author:  Eric Robinson L00709820
  * Date:    10/29/23
 * Lane Community College CS234 Advanced Programming: C# (.NET)
 * Lab 3
 * Code copied from StateDB.cs
 * Purpose: 
 */

using System;
using MMABooksTools;
using MMABooksProps;
using System.Data;

// *** LS: I use an "alias" for the ado.net classes throughout my code.
// When I switch to an Oracle database, I only have to change the actual classes here.
using DBBase = MMABooksTools.BaseSQLDB;
using DBConnection = MySql.Data.MySqlClient.MySqlConnection;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using DBParameter = MySql.Data.MySqlClient.MySqlParameter;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;
using DBDataAdapter = MySql.Data.MySqlClient.MySqlDataAdapter;
using DBDbType = MySql.Data.MySqlClient.MySqlDbType;
using System.Collections.Generic;

namespace MMABooksDB
{
    public class ProductDB : DBBase, IReadDB, IWriteDB // Are we inheriting from 1 class and 2 interfaces?
    {
        public ProductDB() : base() { }
        public ProductDB(DBConnection cn) : base(cn) { }

        public IBaseProps Create(IBaseProps p)
        {
            int rowsAffected = 0;
            ProductProps props = (ProductProps)p;

            // Could this be any more confusing?
            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductCreate"; // usp = User Stored Procedure?
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("ProductCode", props.ProductCode);
            command.Parameters.AddWithValue("Description", props.Description);
            command.Parameters.AddWithValue("UnitPrice", props.UnitPrice);
            command.Parameters.AddWithValue("OnHandQuantity", props.OnHandQuantity);

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.ConcurrencyID = 1;
                    return props;
                }
                else
                    throw new Exception("Unable to insert record. " + props.GetState());
            }
            catch (Exception e)
            {
                // log this error
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        } // end Create()

        public bool Delete(IBaseProps p)
        {
            // This method was not implemented in the CustomerDB class - at least not on the video.
            throw new NotImplementedException();
        } 

        public IBaseProps Retrieve(object key)
        {
            DBDataReader data = null;
            StateProps props = new StateProps();
            DBCommand command = new DBCommand();

            command.CommandText = "usp_ProductSelect";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("ProductCode", DBDbType.VarChar);
            command.Parameters["ProductCode"].Value = (string)key; // key is an object in the method signature - we need to make it a string ProdcutCode.

            try
            {
                data = RunProcedure(command);
                if (!data.IsClosed)
                {
                    if (data.Read())
                    {
                        props.SetState(data);
                    }
                    else
                        throw new Exception("Record does not exist in the database.");
                }
                return props;
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (data != null)
                {
                    if (!data.IsClosed)
                        data.Close();
                }
            }
        } // end Retrieve()

        public object RetrieveAll()
        {
            List<ProductProps> list = new List<ProductProps>();
            DBDataReader reader = null;
            ProductProps props;

            try
            {
                reader = RunProcedure("usp_ProductSelectAll");
                if (!reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        props = new ProductProps();
                        props.SetState(reader);
                        list.Add(props);
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
        } // end RetrieveAll()

        public bool Update(IBaseProps p)
        {
            int rowsAffected = 0;
            ProductProps props = (ProductProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductUpdate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("ProductCode", DBDbType.VarChar);
            command.Parameters.Add("Description", DBDbType.VarChar);
            command.Parameters.Add("UnitPrice", DBDbType.Decimal);
            command.Parameters.Add("OnHAndQuantity", DBDbType.Int32);
            command.Parameters.Add("conCurrId", DBDbType.Int32);
            command.Parameters["ProductCode"].Value = props.ProductCode;
            command.Parameters["Description"].Value = props.Description;
            command.Parameters["UnitPrice"].Value = props.UnitPrice;
            command.Parameters["OnHandQuantity"].Value = props.OnHandQuantity;
            command.Parameters["conCurrId"].Value = props.ConcurrencyID;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.ConcurrencyID++;
                    return true;
                }
                else
                {
                    string message = "Record cannot be updated. It has been edited by another user.";
                    throw new Exception(message);
                }
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        } // end Update()

    } // end class ProductDB : DBBase, IReadDB, IWriteDB
} // end namespace MMABooksDB
