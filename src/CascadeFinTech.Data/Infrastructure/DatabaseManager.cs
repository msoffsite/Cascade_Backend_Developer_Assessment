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
    }
}
