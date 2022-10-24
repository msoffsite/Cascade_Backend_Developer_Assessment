using CascadeFinTech.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using CascadeFinTech.Data.Extensions;

namespace CascadeFinTech.Data.dbo.Publisher
{
    internal class Table : ITable<Model>
    {
        internal string ConnectionString { get; }
        
        private List<SqlParameter> _parameters;

        internal Table(string connectionString)
        {
            ConnectionString = connectionString;
        }

        internal async Task<Model> GetPublisherByIdAsync(Guid inputId)
        {
            _parameters = new List<SqlParameter>
            {
                new SqlParameter("Id", inputId)
            };

            Model result = null;
            using (var reader = await DatabaseManager.ExecuteReaderAsync(
                       StoredProcedure.GetPublisherById,
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

        internal async Task<List<Model>> GetPublishersAsync()
        {
            var output = new List<Model>();
            using (var reader = await DatabaseManager.ExecuteReaderAsync(
                       StoredProcedure.GetPublishers,
                       _parameters,
                       ConnectionString
                   ))
            {
                while (reader.Read())
                {
                    var outputItem = DataReader(reader);
                    if (outputItem != null) { output.Add(outputItem); }
                }
            };
            return output;
        }

        private static Model DataReader(IDataReader reader)
        {
            var output = new Model
            {
                Id = reader.GetValue<Guid>(Field.Id),
                Name = reader.GetValue<string>(Field.Name)
            };

            return output;
        }

        Model ITable<Model>.DataReader(IDataReader reader)
        {
            return DataReader(reader);
        }
    }
}
