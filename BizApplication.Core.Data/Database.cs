using System;
using System.Collections.Generic;
using System.Data;
using BizApplication.Core.Data.CoreIF;

namespace BizApplication.Core.Data
{
    public abstract class Database : IDatabase
    {
        public IDbConnection Connection { get; private set; }
        public IDbTransaction Transaction { get; private set; }

        public void Begin()
        {
            if (HasTransaction())
            {
                throw new InvalidOperationException("Transaction has already begun");
            }

            Transaction = Connection.BeginTransaction();
        }

        public void Commit()
        {
            if (NoExistTransaction())
            {
                throw new InvalidOperationException("Transaction does not exist");
            }
            
            try
            {
                Transaction.Commit();
            }
            catch (Exception e)
            {
                Rollback();
            }
            finally
            {
                DestroyTransaction();
            }
        }

        public void Rollback(Exception causeException = null)
        {
            if (NoExistTransaction())
            {
                throw new InvalidOperationException("Transaction does not exist");
            }
            
            try
            {
                Transaction.Rollback();
            }
            catch (Exception e)
            {
                throw new Exception("Roll back failed", causeException ?? e);
            }
            finally
            {
                DestroyTransaction();
            }
        }

        public int ExecuteCommand(string sql, object param)
        {
            var command = Connection.CreateCommand();
            command.CommandText = sql;
            command.ExecuteNonQuery();

            var parameter = command.CreateParameter();

            throw new NotImplementedException();
        }

        public IEnumerable<dynamic> ExecuteQuery(string sql, object param)
        {
            throw new NotImplementedException();
        }

        public dynamic ExecuteStoredFunction(string sql, object param)
        {
            throw new NotImplementedException();
        }

        public void ExecuteStoredProcedure(string sql, object param)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Determine whether there is a transaction
        /// </summary>
        /// <returns>Returns true if there is a transaction, false otherwise</returns>
        protected bool HasTransaction()
        {
            return !ReferenceEquals(Transaction, null);
        }

        /// <summary>
        /// Determine whether a transaction does not exist
        /// </summary>
        /// <returns>Returns true if there is no transaction, false otherwise</returns>
        protected bool NoExistTransaction()
        {
            return !HasTransaction();
        }

        
        /// <summary>
        /// Destroy the transaction
        /// </summary>
        protected void DestroyTransaction()
        {
            if (NoExistTransaction()) { return; }

            Transaction.Dispose();
            Transaction = null;
        }

        protected void BindParameters(IDbCommand command, object param)
        {
            
        }
    }
}
