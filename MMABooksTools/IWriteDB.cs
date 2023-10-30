/* Author:  LindyStewart
 * Changes: Eric Robinson L00709820
 * Date:    10/23/23
 * Lane Community College CS234 Advanced Programming: C# (.NET)
 * Lab 3
 * Purpose: Establish IWriteDB
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace MMABooksTools
{
    /// <summary>
    /// IWriteDB is part of the "data tier" of the framework.
    /// It supports writing to the data source.
    /// </summary>
    public interface IWriteDB
    {
        IBaseProps Create(IBaseProps props);
        bool Update(IBaseProps props);
        bool Delete(IBaseProps props);

    } // end interface IWriteDB
} // end namespace MMABooksTools
