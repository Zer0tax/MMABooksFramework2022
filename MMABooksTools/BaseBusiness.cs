/* Author:  LindyStewart
 * Changes: Eric Robinson L00709820
 * Date:    10/23/23
 * Lane Community College CS234 Advanced Programming: C# (.NET)
 * Lab 3
 * Purpose: Establish BaseBusiness class
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace MMABooksTools
{
    /// <summary>
    /// BaseBusiness is the class from which all business classes will be derived.
    /// Derived classes must implement constructors and property procedures as well as
    /// the 2 abstract methods SetRequiredRules and SetDefaultProperties.
    /// This class relies on the implementation of corresponding property and data access classes.
    /// </summary>
    public abstract class BaseBusiness
    {
        // These methods are called by the constructor.  The base class doesn't know how to 
        // implement them BUT derived classes must write them so that the contructor can call them.
        protected abstract void SetRequiredRules();
        protected abstract void SetDefaultProperties();
        protected abstract void SetUp();

        public abstract object GetList();

        // Serializable properties of the object
        // Extra copy so that the user can "undo" editing changes before they're sent to the db
        protected IBaseProps mProps;
        protected IBaseProps mOldProps;

        // Data tier objects
        protected IReadDB mdbReadable;
        protected IWriteDB mdbWriteable;

        // Editing state of the object
        protected bool mIsNew = true;
        protected bool mIsDeleted = false;
        protected bool mIsDirty = false;

        // Collection of business rules that are currently "broken"
        // Used to determine if its safe to save the object to the databse
        protected BrokenRules mRules = new BrokenRules();

        // Desktop apps generally retrieve connection string information from the config file when the user logs in.
        // DB objects don't have to get the connection string themselves if the ui objects pass it as a parameter
        // in the call to the constructor.
        protected string mConnectionString = "";

        // Creates a new business object.
        // Default constructor
        public BaseBusiness()
        {
            SetUp();
            SetRequiredRules();
            SetDefaultProperties();
        }

        public BaseBusiness(object key)
        {
            SetUp();
            Load(key);
        }

        public BaseBusiness(IBaseProps props)
        {
            SetUp();
            LoadProps(props);
        }

        // Editing state of the business object
        public bool IsNew
        {
            get
            {
                return mIsNew;
            }
        }

        public bool IsDeleted
        {
            get
            {
                return mIsDeleted;
            }
        }

        public virtual bool IsDirty
        {
            get
            {
                return mIsDirty;
            }
        }

        public bool IsValid
        {
            get
            {
                return (mRules.Count == 0); // ???
            }
        }

        public override string ToString()
        {
            return mProps.GetState(); // ???
        }

        public virtual void UndoChanges()
        {
            if (mIsDirty || mIsDeleted)
            {
                mProps = (IBaseProps)mOldProps.Clone(); // Copy from backup copy
                mIsDirty = false;
                mIsDeleted = false;
                if (mIsNew)
                    SetRequiredRules(); // ???
                else
                    mRules.Clear(); // ???
            }
        }

        // Loads the object from the database based on its key.
        public virtual void Load(Object key)
        {
            mProps = mdbReadable.Retrieve(key);
            mOldProps = (IBaseProps)mProps.Clone(); // Make a backup copy.

            mIsDirty = false;
            mIsNew = false;
            mIsDeleted = false;

            mRules.Clear();
        }

        // Loads from an xml string.
        public virtual void LoadXML(string xml)
        {
            mProps.SetState(xml);
            mOldProps = (IBaseProps)mProps.Clone();

            mIsDirty = false;
            mIsNew = false;
            mIsDeleted = false;

            mRules.Clear();
        }

        // Loads from a properties object.
        public virtual void LoadProps(IBaseProps props)
        {
            mProps = (IBaseProps)props.Clone();
            mOldProps = (IBaseProps)props.Clone();

            mIsDirty = false;
            mIsNew = false;
            mIsDeleted = false;

            mRules.Clear();
        }

        // Saves to a data store. 
        public virtual void Save()
        {
            if (mIsDeleted && !mIsNew)
            {
                if (mdbWriteable.Delete(mProps))
                {
                    mIsDeleted = false;
                    mIsNew = true;
                    mIsDirty = false;
                    SetRequiredRules();
                    SetDefaultProperties();
                    mOldProps = (IBaseProps)mProps.Clone();
                }
            }
            else if (mIsDeleted && mIsNew)
            {
                mIsDeleted = false;
                mIsNew = true;
                mIsDirty = false;
                SetRequiredRules();
                SetDefaultProperties();
                mOldProps = (IBaseProps)mProps.Clone();
            }
            else if (!IsValid)
            {
                string message;
                if (mRules.Count == 1)
                {
                    message = "Object cannot be saved. One property is invalid.";
                }
                else
                {
                    message = "Object can not be saved. " + mRules.Count + " properties are invalid.";
                }
                throw new Exception(message);
            }
            else if (mIsNew && !mIsDeleted)
            {
                mProps = mdbWriteable.Create(mProps);
                mIsNew = false;
                mIsDirty = false;
                mIsDeleted = false;
                mRules.Clear();
                mOldProps = (IBaseProps)mProps.Clone();
            }
            else if (IsDirty)
            {
                if (mdbWriteable.Update(mProps))
                {
                    mIsDirty = false;
                    mIsNew = false;
                    mIsDeleted = false;
                    mRules.Clear();
                    mOldProps = (IBaseProps)mProps.Clone();
                }
            } // end logic related to editing status
        }// end Save() 

        /// <summary>
        /// This method only marks the object for deletion. To actually delete it from the DB, 
        /// you must call Save() after calling Delete().
        /// </summary>
        public void Delete()
        {
            mIsDeleted = true;
        }

    } // end class BaseBusiness
} // end namespace MMABooksTools
