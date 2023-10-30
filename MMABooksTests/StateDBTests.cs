/* Author:  LindyStewart
 * Changes: Eric Robinson L00709820
 * Date:    10/25/23
 * Lane Community College CS234 Advanced Programming: C# (.NET)
 * Lab 3
 * Purpose: 
 */

using NUnit.Framework;
using MMABooksProps;
using MMABooksDB;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;
using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace MMABooksTests
{
    [TestFixture]
    public class StateDBTests
    {
        StateDB db;

        [SetUp]
        // This method runs before every test.
        public void ResetData()
        {
            db = new StateDB(); // How is our .JSON file referenced to get MySQL DB password?
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieve()
        {
            StateProps p = (StateProps)db.Retrieve("OR"); // We cast as StateProps.
            Assert.AreEqual("OR", p.Code);
            Assert.AreEqual("Ore", p.Name); // There is an error in our DB. It is "Ore" not "Oregon".
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<StateProps> list = (List<StateProps>)db.RetrieveAll(); // Cast as List<StateProps>.
            Assert.AreEqual(53, list.Count);
        }

        [Test]
        public void TestDelete()
        {
            StateProps p = (StateProps)db.Retrieve("HI"); // We cast as StateProps.
            
            // The Delete() method should return TRUE if 1 row has been deleted.
            Assert.True(db.Delete(p)); 
            
            // We just deleted "HI". If we try to retrieve it, we should get an error.
            Assert.Throws<Exception>(() => db.Retrieve("HI"));
        }
         
        [Test]
        public void TestDeleteForeignKeyConstraint()
        {
            StateProps p = (StateProps)db.Retrieve("OR");
            Assert.Throws<MySqlException>(() => db.Delete(p));
        }

        [Test]
        public void TestUpdate()
        {
            StateProps p = (StateProps)db.Retrieve("CA");
            p.Name = "Canada";
            Assert.True(db.Update(p));
            p = (StateProps)db.Retrieve("CA");
            Assert.AreEqual("Canada", p.Name);
        }

        [Test]
        public void TestUpdateFieldTooLong()
        {
            StateProps p = (StateProps)db.Retrieve("OR");
            p.Name = "Oregon is the state where Crater Lake National Park is.";
            Assert.Throws<MySqlException>(() => db.Update(p));
        }

        [Test]
        public void TestCreate()
        {
            StateProps p = new StateProps();
            p.Code = "??";
            p.Name = "Where am I";
            db.Create(p);
            StateProps p2 = (StateProps)db.Retrieve(p.Code);
            Assert.AreEqual(p.GetState(), p2.GetState());
        }

        [Test]
        public void TestCreatePrimaryKeyViolation()
        {
            // We cant add a state if it already exists.
            StateProps p = new StateProps();
            p.Code = "OR";
            p.Name = "Oregon";
            Assert.Throws<MySqlException>(() => db.Create(p));
        }

    } // end class StateDBTests
} // end namespace MMABooksTests