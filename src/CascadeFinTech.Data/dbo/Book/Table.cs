using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CascadeFinTech.Data.Extensions;
using CascadeFinTech.Data.Infrastructure;

using StoredProcedure = CascadeFinTech.Data.Infrastructure.StoredProcedure;

namespace CascadeFinTech.Data.dbo.Book
{
    internal class Table : ITable<Model>
    {
        internal string ConnectionString { get; }

        private List<SqlParameter> _parameters;

        internal Table(string connectionString)
        {
            ConnectionString = connectionString;
        }

        internal async Task<List<Model>> GetBooksAsync()
        {
            var output = new List<Model>();
            using (var reader = await DatabaseManager.ExecuteReaderAsync(
                       StoredProcedure.GetBooks,
                       _parameters,
                       ConnectionString
                   ))
            {
                var table = new Model();
                while (reader.Read())
                {
                    var outputItem = DataReader(reader);
                    if (outputItem != null) { output.Add(outputItem); }
                }
            };
            return output;
        }

        internal async Task<List<Model>> GetBooksSortedByAuthorLastFirstPublisherAsync()
        {
            var result = new List<Model>();
            using (var reader = await DatabaseManager.ExecuteReaderAsync(
                       StoredProcedure.GetBooksSortedByAuthorLastFirstPublisher,
                       _parameters,
                       ConnectionString
                   ))
            {
                var table = new Model();
                while (reader.Read())
                {
                    var item = DataReader(reader);
                    if (item != null) { result.Add(item); }
                }
            };
            return result;
        }

        internal async Task<List<Model>> GetBooksSortedByPublisherAuthorLastFirstAsync()
        {
            var result = new List<Model>();
            using (var reader = await DatabaseManager.ExecuteReaderAsync(
                       StoredProcedure.GetBooksSortedByPublisherAuthorLastFirst,
                       _parameters,
                       ConnectionString
                   ))
            {
                var table = new Model();
                while (reader.Read())
                {
                    var item = DataReader(reader);
                    if (item != null) { result.Add(item); }
                }
            };
            return result;
        }

        private static Model DataReader(IDataReader reader)
        {
            var output = new Model
            {
                Id = reader.GetValue<Guid>(Field.Id),
                AuthorId = reader.GetValue<Guid>(Field.AuthorId),
                PublisherId = reader.GetValue<Guid>(Field.PublisherId),
                Title = reader.GetValue<string>(Field.Title)
            };

            return output;
        }

        Model ITable<Model>.DataReader(IDataReader reader)
        {
            return DataReader(reader);
        }
    }
}
