/* Author:  Lindy Stewart
 * Changes: Eric Robinson L00709820
 * Date:    10/28/23
 * Lane Community College CS234 Advanced Programming: C# (.NET)
 * Lab 3
 * Purpose: 
 */

using System;
using System.Collections.Generic;
using System.Text;
using MMABooksTools;
using MMABooksProps;
using System.Data;

// We use an "alias" for the ado.net classes throughout my code
// When I switch to an oracle database, I ONLY have to change the actual classes here
using DBBase = MMABooksTools.BaseSQLDB;
using DBConnection = MySql.Data.MySqlClient.MySqlConnection;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using DBParameter = MySql.Data.MySqlClient.MySqlParameter;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;
using DBDataAdapter = MySql.Data.MySqlClient.MySqlDataAdapter;
using DBDbType = MySql.Data.MySqlClient.MySqlDbType;
using Google.Protobuf.WellKnownTypes;

namespace MMABooksDB
{
    public class CustomerDB : DBBase, IReadDB, IWriteDB // Are we inheriting from 1 class and 2 interfaces?
    {
        
        public IBaseProps Create(IBaseProps p)
        {
            int rowsAffected = 0;
            CustomerProps props = (CustomerProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_CustomerCreate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("custId", DBDbType.Int32);
            command.Parameters.Add("name_p", DBDbType.VarChar);
            command.Parameters.Add("Address_p", DBDbType.VarChar);
            command.Parameters.Add("City_p", DBDbType.VarChar);
            command.Parameters.Add("State_p", DBDbType.VarChar);
            command.Parameters.Add("ZipCode_p", DBDbType.VarChar);
            
            // ... there are more parameters here
            // No idea what happens here!
            command.Parameters[0].Direction = ParameterDirection.Output;
            command.Parameters["name_p"].Value = props.Name;
            command.Parameters["Address_p"].Value = props.Address;
            command.Parameters["City_p"].Value = props.City;
            command.Parameters["State_p"].Value = props.State;
            command.Parameters["ZipCode_p"].Value = props.ZipCode;
            // ... and more values here

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.CustomerID = (int)command.Parameters[0].Value;
                    props.ConcurrencyID = 1;
                    return props;
                }
                else
                    throw new Exception("Unable to insert record. " + props.ToString());
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
        }

        public bool Delete(IBaseProps props)
        {
            throw new NotImplementedException();
        }

        public IBaseProps Retrieve(object key) // We dont know type what our PK is, so we made it an object.
        {
            DBDataReader data = null;
            CustomerProps props = new CustomerProps();
            DBCommand command = new DBCommand();

            command.CommandText = "usp_CustomerSelect";
            command.CommandType = CommandType.StoredProcedure;
            
            // We use "custID" because that is what is in usp_CustomerSelect in MySQL.
            command.Parameters.Add("custID", DBDbType.Int32);
            // key is an object in the method signature - we need to make it an int here.
            command.Parameters["custID"].Value = key.ToString();

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
        }

        public object RetrieveAll()
        {
            List<CustomerProps> list = new List<CustomerProps>();
            DBDataReader reader = null;
            CustomerProps props;

            try
            {
                reader = RunProcedure("usp_CustomerSelectAll");
                if (!reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        props = new CustomerProps();
                        props.SetCustomer(reader);
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

        public bool Update(IBaseProps props)
        {
            throw new NotImplementedException();
        } // end Update()

    } // end class Customer DB
} // end namespace MMABooksDB
