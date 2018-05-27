using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace BizApplication.Core.Data.CoreIF
{
    public interface IDatabaseFactory
    {
        /// Build a connection string
        /// </summary>
        /// <param name="dbConnectionStringBuilder">Connection string builder</param>
        /// <returns>Connection string</returns>
        string CreateConnectionString(DbConnectionStringBuilder dbConnectionStringBuilder);

        /// <summary>
        /// Create a database object
        /// </summary>
        /// <returns>Database object</returns>
        IDatabase CreateDatabase();

        /// <summary>
        /// Destroy a database object
        /// </summary>
        /// <param name="database">Database object</param>
        void DestryDatabase(IDatabase database);
    }
}
