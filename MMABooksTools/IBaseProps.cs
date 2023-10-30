/* Author:  LindyStewart
 * Changes: Eric Robinson L00709820
 * Date:    10/23/23
 * Lane Community College CS234 Advanced Programming: C# (.NET)
 * Lab 3
 * Purpose: Establish IBaseProps (whatever they are)
 */

using System;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;

namespace MMABooksTools
{
    /// <summary>
    /// IBaseProps is the "middle tier" of the framework.
    /// It converts data tier attribute values into business tier attribute values.
    /// </summary>
    public interface IBaseProps : ICloneable
    {
        string GetState();

        void SetState(string jsonString);
        void SetState(DBDataReader dr);
    
    } // end interface IBaseProps : ICloneable
} // end namespace MMABooksTools
