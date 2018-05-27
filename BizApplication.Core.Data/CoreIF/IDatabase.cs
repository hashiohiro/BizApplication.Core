using System;
using System.Collections.Generic;

namespace BizApplication.Core.Data.CoreIF
{
    /// <summary>
    /// Encapsulate database-specific functions and rules
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Begin transaction
        /// </summary>
        void Begin();

        /// <summary>
        /// Commit the transaction 
        /// </summary>
        void Commit();

        /// <summary>
        /// Roll back the transaction
        /// </summary>
        /// <param name="causeException">cause exception</param>
        void Rollback(Exception causeException = null);

        /// <summary>
        /// Execute a query
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">parameters</param>
        /// <returns>query result</returns>
        IEnumerable<dynamic> ExecuteQuery(string sql, object param);

        /// <summary>
        /// Execute a command
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">parameters</param>
        /// <returns>command result</returns>
        int ExecuteCommand(string sql, object param);

        /// <summary>
        /// Execute a stored function
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">parameters</param>
        /// <returns>function result</returns>
        dynamic ExecuteStoredFunction(string sql, object param);

        /// <summary>
        /// Execute a stored procedure
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">parameters</param>
        /// <returns>procedure result</returns>
        void ExecuteStoredProcedure(string sql, object param);
    }
}