using CascadeFinTech.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CascadeFinTech.Data.Infrastructure
{
    internal sealed class DatabaseManager
    {
        private const int CommandTimeout = 300;
        private static readonly DbProviderFactory DbFactory;

        static DatabaseManager()
        {
            DbFactory = SqlClientFactory.Instance;
        }

        private static DbConnection GetConnection(string connectionString)
        {
            DbConnection connection = DbFactory.CreateConnection();
            if (connection == null)
                return null;

            connection.ConnectionString = connectionString;
            return connection;
        }

        private static DbCommand GetCommand(string dbObjectName, CommandType dbType, DbConnection connection, IEnumerable<SqlParameter> paramValues, int commandTimeout)
        {
            DbCommand cmd = DbFactory.CreateCommand();

            if (cmd == null)
                return null;

            cmd.CommandTimeout = commandTimeout;
            cmd.CommandText = dbObjectName;
            cmd.CommandType = dbType;
            cmd.Connection = connection;

            if (paramValues != null)
            {
                foreach (var val in paramValues)
                {
                    if (val.Value == null) val.Value = DBNull.Value;
                    cmd.Parameters.Add(val);
                }
            }

            return cmd;
        }

        private static DbDataAdapter GetDataAdapter(DbCommand cmd)
        {
            var adapter = DbFactory.CreateDataAdapter();
            if (adapter == null)
                return null;

            adapter.SelectCommand = cmd;
            return adapter;
        }

        /// <summary>
        /// Executes the SQL statement and returns a DataSet containing the results returned by the database.
        /// </summary>
        /// <param name="dbObjectName">The sql string that should be executed on the database. Can be a string of dynamic sql or a stored procedure name.</param>
        /// <param name="paramValues">An enumerable of SqlParameter that represents the parameters needed for the sql statement</param>
        /// <param name="dbType">Optional parameter, defaults to Stored Procedure. Set this to the type of SQL command being executed (CommandType.Text or CommandType.StoredProcedure).</param>
        /// <param name="connectionString">Optional parameter, defaults to "evestment". The name of the database connection string to use for this sql statement.</param>
        /// <param name="commandTimeout">Optional parameter, defaults to 300 seconds. The timeout period for this sql statement.</param>
        /// <returns>A DataSet containing the results of the query.</returns>
        internal static DataSet ExecuteDataSet(string dbObjectName,
            IEnumerable<SqlParameter> paramValues,
            string connectionString,
            CommandType dbType = CommandType.StoredProcedure,
            int commandTimeout = CommandTimeout)
        {
            DataSet ds;
            using (var conn = GetConnection(connectionString))
            {
                using var cmd = GetCommand(dbObjectName, dbType, conn, paramValues, commandTimeout);
                using var adapter = GetDataAdapter(cmd);
                ds = new DataSet();
                if (adapter != null) adapter.Fill(ds);
            }
            return ds;
        }

        /// <summary>
        /// Asynchronously executes the SQL statement and returns a DataSet containing the results returned by the database.
        /// </summary>
        /// <param name="dbObjectName">The sql string that should be executed on the database. Can be a string of dynamic sql or a stored procedure name.</param>
        /// <param name="paramValues">An enumerable of SqlParameter that represents the parameters needed for the sql statement</param>
        /// <param name="dbType">Optional parameter, defaults to Stored Procedure. Set this to the type of SQL command being executed (CommandType.Text or CommandType.StoredProcedure).</param>
        /// <param name="connectionString">Optional parameter, defaults to "evestment". The name of the database connection string to use for this sql statement.</param>
        /// <param name="commandTimeout">Optional parameter, defaults to 300 seconds. The timeout period for this sql statement.</param>
        /// <returns>A DataSet containing the results of the query.</returns>
        internal static async Task<DataSet> ExecuteDataSetAsync(string dbObjectName,
            IEnumerable<SqlParameter> paramValues,
            string connectionString,
            CommandType dbType = CommandType.StoredProcedure,
            int commandTimeout = CommandTimeout)
        {
            DataSet ds;
            using (var conn = GetConnection(connectionString))
            {
                using var cmd = GetCommand(dbObjectName, dbType, conn, paramValues, commandTimeout);
                using var adapter = GetDataAdapter(cmd);
                ds = new DataSet();
                if (adapter != null) adapter.Fill(ds);
            }
            return await Task.FromResult(ds).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the SQL statement and returns the first DataTable returned by that statement.
        /// </summary>
        /// <param name="dbObjectName">The sql string that should be executed on the database. Can be a string of dynamic sql or a stored procedure name.</param>
        /// <param name="paramValues">An enumerable of SqlParameter that represents the parameters needed for the sql statement</param>
        /// <param name="dbType">Optional parameter, defaults to Stored Procedure. Set this to the type of SQL command being executed (CommandType.Text or CommandType.StoredProcedure).</param>
        /// <param name="connectionString">Optional parameter, defaults to "evestment". The name of the database connection string to use for this sql statement.</param>
        /// <param name="commandTimeout">Optional parameter, defaults to 300 seconds. The timeout period for this sql statement.</param>
        /// <returns>A DataTable containing the first table of the results of the query.</returns>
        internal static DataTable ExecuteDataTable(string dbObjectName,
            IEnumerable<SqlParameter> paramValues,
            string connectionString,
            CommandType dbType = CommandType.StoredProcedure,
            int commandTimeout = CommandTimeout)
        {
            DataTable dt = null;
            var ds = ExecuteDataSet(dbObjectName, paramValues, connectionString, dbType, commandTimeout);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }

            return dt;
        }

        /// <summary>
        /// Asynchronously executes the SQL statement and returns the first DataTable returned by that statement.
        /// </summary>
        /// <param name="dbObjectName">The sql string that should be executed on the database. Can be a string of dynamic sql or a stored procedure name.</param>
        /// <param name="paramValues">An enumerable of SqlParameter that represents the parameters needed for the sql statement</param>
        /// <param name="dbType">Optional parameter, defaults to Stored Procedure. Set this to the type of SQL command being executed (CommandType.Text or CommandType.StoredProcedure).</param>
        /// <param name="connectionString">Optional parameter, defaults to "evestment". The name of the database connection string to use for this sql statement.</param>
        /// <param name="commandTimeout">Optional parameter, defaults to 300 seconds. The timeout period for this sql statement.</param>
        /// <returns>A DataTable containing the first table of the results of the query.</returns>
        internal static async Task<DataTable> ExecuteDataTableAsync(string dbObjectName,
            IEnumerable<SqlParameter> paramValues,
            string connectionString,
            CommandType dbType = CommandType.StoredProcedure,
            int commandTimeout = CommandTimeout)
        {
            var dt = new DataTable();
            var reader = await ExecuteReaderAsync(dbObjectName, paramValues, connectionString, dbType, commandTimeout).ConfigureAwait(false);

            dt.Load(reader);

            return dt;
        }

        /// <summary>
        /// Executes the SQL statement and returns an IDataReader. WARNING: To prevent memory leaks please 
        /// use a 'using (var reader = DatabaseManager.ExecuteReader()) { }' statement.
        /// </summary>
        /// <param name="dbObjectName">The sql string that should be executed on the database. Can be a string of dynamic sql or a stored procedure name.</param>
        /// <param name="paramValues">An enumerable of SqlParameter that represents the parameters needed for the sql statement</param>
        /// <param name="dbType">Optional parameter, defaults to Stored Procedure. Set this to the type of SQL command being executed (CommandType.Text or CommandType.StoredProcedure).</param>
        /// <param name="connectionString">Optional parameter, defaults to "evestment". The name of the database connection string to use for this sql statement.</param>
        /// <param name="commandTimeout">Optional parameter, defaults to 300 seconds. The timeout period for this sql statement.</param>
        /// <returns>An IDataReader for reading the results of the query.</returns>
        internal static IDataReader ExecuteReader(string dbObjectName,
            IEnumerable<SqlParameter> paramValues,
            string connectionString,
            CommandType dbType = CommandType.StoredProcedure,
            int commandTimeout = CommandTimeout)
        {
            var conn = GetConnection(connectionString);
            conn.Open();

            var cmd = GetCommand(dbObjectName, dbType, conn, paramValues, commandTimeout);
            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            return reader;
        }

        /// <summary>
        /// Asynchronously executes the SQL statement and returns an IDataReader. WARNING: To prevent memory leaks please 
        /// use a 'using (var reader = DatabaseManager.ExecuteReader()) { }' statement.
        /// </summary>
        /// <param name="dbObjectName">The sql string that should be executed on the database. Can be a string of dynamic sql or a stored procedure name.</param>
        /// <param name="paramValues">An enumerable of SqlParameter that represents the parameters needed for the sql statement</param>
        /// <param name="dbType">Optional parameter, defaults to Stored Procedure. Set this to the type of SQL command being executed (CommandType.Text or CommandType.StoredProcedure).</param>
        /// <param name="connectionString">Optional parameter, defaults to "evestment". The name of the database connection string to use for this sql statement.</param>
        /// <param name="commandTimeout">Optional parameter, defaults to 300 seconds. The timeout period for this sql statement.</param>
        /// <returns>An IDataReader for reading the results of the query.</returns>
        internal static async Task<IDataReader> ExecuteReaderAsync(string dbObjectName,
            IEnumerable<SqlParameter> paramValues,
            string connectionString,
            CommandType dbType = CommandType.StoredProcedure,
            int commandTimeout = CommandTimeout)
        {
            var conn = GetConnection(connectionString);
            conn.Open();

            var cmd = GetCommand(dbObjectName, dbType, conn, paramValues, commandTimeout);
            IDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection).ConfigureAwait(false);

            return reader;
        }

        /// <summary>
        /// Executes the SQL statement and returns a scalar value of type T from the first column of the first row of the results of the statement.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbObjectName">The sql string that should be executed on the database. Can be a string of dynamic sql or a stored procedure name.</param>
        /// <param name="paramValues">An enumerable of SqlParameter that represents the parameters needed for the sql statement</param>
        /// <param name="dbType">Optional parameter, defaults to Stored Procedure. Set this to the type of SQL command being executed (CommandType.Text or CommandType.StoredProcedure).</param>
        /// <param name="connectionString">Optional parameter, defaults to "evestment". The name of the database connection string to use for this sql statement.</param>
        /// <param name="commandTimeout">Optional parameter, defaults to 300 seconds. The timeout period for this sql statement.</param>
        /// <returns>A scalar value of Type T.</returns>
        internal static T ExecuteScalar<T>(string dbObjectName,
            IEnumerable<SqlParameter> paramValues,
            string connectionString,
            CommandType dbType = CommandType.StoredProcedure,
            int commandTimeout = CommandTimeout)
        {
            object dbResult = null;
            using (var conn = GetConnection(connectionString))
            {
                conn.Open();
                using var cmd = GetCommand(dbObjectName, dbType, conn, paramValues, commandTimeout);
                if (cmd != null) dbResult = cmd.ExecuteScalar();
            }

            if (dbResult == null || DBNull.Value.Equals(dbResult)) return default;
            return dbResult.ConvertValue<T>();
        }

        /// <summary>
        /// Asynchronously executes the SQL statement and returns a scalar value of type T from the first column of the first row of the results of the statement.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbObjectName">The sql string that should be executed on the database. Can be a string of dynamic sql or a stored procedure name.</param>
        /// <param name="paramValues">An enumerable of SqlParameter that represents the parameters needed for the sql statement</param>
        /// <param name="dbType">Optional parameter, defaults to Stored Procedure. Set this to the type of SQL command being executed (CommandType.Text or CommandType.StoredProcedure).</param>
        /// <param name="connectionString">Optional parameter, defaults to "evestment". The name of the database connection string to use for this sql statement.</param>
        /// <param name="commandTimeout">Optional parameter, defaults to 300 seconds. The timeout period for this sql statement.</param>
        /// <returns>A scalar value of Type T.</returns>
        internal static async Task<T> ExecuteScalarAsync<T>(string dbObjectName,
            IEnumerable<SqlParameter> paramValues,
            string connectionString,
            CommandType dbType = CommandType.StoredProcedure,
            int commandTimeout = CommandTimeout)
        {
            object dbResult = null;
            using (var conn = GetConnection(connectionString))
            {
                conn.Open();
                using var cmd = GetCommand(dbObjectName, dbType, conn, paramValues, commandTimeout);
                if (cmd != null) dbResult = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
            }

            if (dbResult == null || DBNull.Value.Equals(dbResult)) return default;
            return dbResult.ConvertValue<T>();
        }

        /// <summary>
        /// Executes a SQL statement and returns the number of rows effected by the statement.
        /// </summary>
        /// <param name="dbObjectName">The sql string that should be executed on the database. Can be a string of dynamic sql or a stored procedure name.</param>
        /// <param name="paramValues">An enumerable of SqlParameter that represents the parameters needed for the sql statement</param>
        /// <param name="dbType">Optional parameter, defaults to Stored Procedure. Set this to the type of SQL command being executed (CommandType.Text or CommandType.StoredProcedure).</param>
        /// <param name="connectionString">Optional parameter, defaults to "evestment". The name of the database connection string to use for this sql statement.</param>
        /// <param name="commandTimeout">Optional parameter, defaults to 300 seconds. The timeout period for this sql statement.</param>
        /// <returns>The number of rows effected by the SQL statement</returns>
        internal static int ExecuteNonQuery(string dbObjectName,
            IEnumerable<SqlParameter> paramValues,
            string connectionString,
            CommandType dbType = CommandType.StoredProcedure,
            int commandTimeout = CommandTimeout)
        {
            int returnCount = 0;
            using (var conn = GetConnection(connectionString))
            {
                conn.Open();
                using var cmd = GetCommand(dbObjectName, dbType, conn, paramValues, commandTimeout);
                if (cmd != null) returnCount = cmd.ExecuteNonQuery();
            }
            return returnCount;
        }

        /// <summary>
        /// Executes a series of SQL statements wrapped in a transaction and returns a resultant object.
        /// </summary>
        /// <param name="sqlFunction">A lambda expression that operates within the non query transaction.</param>
        /// <param name="level">Optional parameter, defaults to "System.Transactions.IsolationLevel.Serializable". The isolation level of the transaction.</param>
        /// <param name="connectionString">Optional parameter, defaults to "evestment". The name of the database connection string to use for this sql statement.</param>
        /// <param name="commandTimeout">Optional parameter, defaults to 300 seconds. The timeout period for this sql statement.</param>
        /*internal static object ExecuteTransaction(Func<DbCommand, object> sqlFunction,
                                                System.Transactions.IsolationLevel level = System.Transactions.IsolationLevel.Serializable,
                                                 string connectionString = "",
                                                 int commandTimeout = CommandTimeout)
        {
            object returnObject;

            if (string.IsNullOrEmpty(connectionString)) { connectionString = CASConnectionStringName; }

            var options = new TransactionOptions
            {
                IsolationLevel = level,
                Timeout = TimeSpan.FromSeconds(commandTimeout)
            };

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                using (var conn = GetConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = GetCommand(string.Empty, CommandType.Text, conn, null, commandTimeout))
                    {
                        returnObject = sqlFunction.Invoke(cmd);
                    }
                }

                transactionScope.Complete();
            }

            return returnObject;
        }*/

        /// <summary>
        /// Executes a SQL statement asynchronously.
        /// </summary>
        /// <param name="dbObjectName">The sql string that should be executed on the database. Can be a string of dynamic sql or a stored procedure name.</param>
        /// <param name="paramValues">An enumerable of SqlParameter that represents the parameters needed for the sql statement.</param>
        /// <param name="dbType">Optional parameter, defaults to Stored Procedure. Set this to the type of SQL command being executed (CommandType.Text or CommandType.StoredProcedure).</param>
        /// <param name="connectionString">Optional parameter, defaults to "evestment". The name of the database connection string to use for this sql statement.</param>
        /// <param name="commandTimeout">Optional parameter, defaults to 300 seconds. The timeout period for this sql statement .</param>
        internal static async Task<int> ExecuteNonQueryAsync(string dbObjectName,
            IEnumerable<SqlParameter> paramValues,
            string connectionString,
            CommandType dbType = CommandType.StoredProcedure,
            int commandTimeout = CommandTimeout)
        {
            using (var conn = GetConnection(connectionString))
            {
                conn.Open();
                using var cmd = GetCommand(dbObjectName, dbType, conn, paramValues, commandTimeout);
                if (cmd != null)
                {
                    return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }

            return 0;
        }

        /// <summary>
        /// BulkCopy will allow the bulk insert of data into the database. It will copy the data in the provided datatable directly
        /// into the destination table. For large inserts this will perform better over single inserts statements by a factor of 10 or more.
        /// </summary>
        /// <param name="destinationTableName">the target table to insert the data into (ie: [eA.Analytics.Common].Universes</param>
        /// <param name="tableToWrite">A DataTable containing the data to be inserted. The column names and datatypes should the same as those 
        /// in the destination table.</param>
        /// <param name="columnMappings">Optional parameter, defaults to using a direct mapping of the column names in the tableToWrite to 
        /// the destinationTable. This only works when the column names are the same. If the column names are different then the user should 
        /// provide a list of column mapping objects that map column names in the tableToWrite to column names in the destination table.</param>
        /// <param name="connectionString">Optional parameter, defaults to "evestment". The name of the database connection string to 
        /// use for this sql statement.</param>
        /// <param name="commandTimeout">Optional parameter, defaults to 300 seconds. The timeout period for this sql statement.</param>
        internal static void BulkCopy(string destinationTableName,
            DataTable tableToWrite,
            string connectionString,
            List<SqlBulkCopyColumnMapping> columnMappings = null,
            int commandTimeout = CommandTimeout)
        {
            if (string.IsNullOrEmpty(destinationTableName))
                throw new ArgumentNullException("destinationTableName", "destinationTableName cannot be null");

            if (tableToWrite == null)
                throw new ArgumentNullException("tableToWrite", "tableToWrite cannot be null");

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var bulkCopy = new SqlBulkCopy(conn);
            bulkCopy.BatchSize = 500;
            bulkCopy.BulkCopyTimeout = 900;

            bulkCopy.DestinationTableName = destinationTableName;

            if (columnMappings == null)
            {
                foreach (DataColumn column in tableToWrite.Columns)
                {
                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }
            }
            else
            {
                foreach (var cm in columnMappings)
                {
                    bulkCopy.ColumnMappings.Add(cm);
                }
            }

            bulkCopy.WriteToServer(tableToWrite);
        }

        /// <summary>
        /// BulkCopy will allow the bulk insert of data into the database asynchronously. It will copy the data in the provided datatable directly
        /// into the destination table. For large inserts this will perform better over single inserts statements by a factor of 10 or more.
        /// </summary>
        /// <param name="destinationTableName">the target table to insert the data into (ie: [eA.Analytics.Common].Universes</param>
        /// <param name="tableToWrite">A DataTable containing the data to be inserted. The column names and datatypes should the same as those 
        /// in the destination table.</param>
        /// <param name="columnMappings">Optional parameter, defaults to using a direct mapping of the column names in the tableToWrite to 
        /// the destinationTable. This only works when the column names are the same. If the column names are different then the user should 
        /// provide a list of column mapping objects that map column names in the tableToWrite to column names in the destination table.</param>
        /// <param name="connectionString">Optional parameter, defaults to "evestment". The name of the database connection string to 
        /// use for this sql statement.</param>
        /// <param name="commandTimeout">Optional parameter, defaults to 300 seconds. The timeout period for this sql statement.</param>
        internal static async Task BulkCopyAsync(string destinationTableName,
            DataTable tableToWrite,
            string connectionString,
            List<SqlBulkCopyColumnMapping> columnMappings = null,
            int commandTimeout = CommandTimeout)
        {
            if (string.IsNullOrEmpty(destinationTableName))
                throw new ArgumentNullException("destinationTableName", "destinationTableName cannot be null");

            if (tableToWrite == null)
                throw new ArgumentNullException("tableToWrite", "tableToWrite cannot be null");

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var bulkCopy = new SqlBulkCopy(conn);
            bulkCopy.BatchSize = 500;
            bulkCopy.BulkCopyTimeout = 900;

            bulkCopy.DestinationTableName = destinationTableName;

            if (columnMappings == null)
            {
                foreach (DataColumn column in tableToWrite.Columns)
                {
                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }
            }
            else
            {
                foreach (var cm in columnMappings)
                {
                    bulkCopy.ColumnMappings.Add(cm);
                }
            }

            await bulkCopy.WriteToServerAsync(tableToWrite).ConfigureAwait(false);
        }
    }
}
