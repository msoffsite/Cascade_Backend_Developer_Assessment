﻿using CascadeFinTech.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using CascadeFinTech.Data.Extensions;

namespace CascadeFinTech.Data.dbo.Author
{
    internal class Table : ITable<Model>
    {
        internal string ConnectionString { get; }
        
        private List<SqlParameter> _parameters;

        internal Table(string connectionString)
        {
            ConnectionString = connectionString;
        }

        internal async Task<Model> GetAuthorByIdAsync(Guid inputId)
        {
            _parameters = new List<SqlParameter>
            {
                new SqlParameter("Id", inputId)
            };

            Model result = null;
            using (var reader = await DatabaseManager.ExecuteReaderAsync(
                       StoredProcedure.GetAuthorById,
                       _parameters,
                       ConnectionString
                   ))
            {
                while (reader.Read())
                {
                    result = DataReader(reader);
                }
            };
            return result;
        }

        private static Model DataReader(IDataReader reader)
        {
            var output = new Model
            {
                Id = reader.GetValue<Guid>(Field.Id),
                FirstName = reader.GetValue<string>(Field.FirstName),
                LastName = reader.GetValue<string>(Field.LastName)
            };

            return output;
        }

        Model ITable<Model>.DataReader(IDataReader reader)
        {
            return DataReader(reader);
        }
    }
}
