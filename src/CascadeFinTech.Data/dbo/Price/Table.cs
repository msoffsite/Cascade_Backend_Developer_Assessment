using CascadeFinTech.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using CascadeFinTech.Data.Extensions;

namespace CascadeFinTech.Data.dbo.Price
{
    internal class Table : ITable<Model>
    {
        internal string ConnectionString { get; }
        private List<SqlParameter> _parameters;

        internal Table(string connectionString)
        {
            ConnectionString = connectionString;
        }

        internal async Task<Model> GetPriceByBookIdCurrencyAsync(Guid bookId, Enumeration.Currency currency)
        {
            _parameters = new List<SqlParameter>
            {
                new SqlParameter("BookId", bookId),
                new SqlParameter("Currency", currency.ToString())
            };

            Model output = null;
            using (var reader = await DatabaseManager.ExecuteReaderAsync(
                       StoredProcedure.GetPriceByBookIdCurrency,
                       _parameters,
                       ConnectionString
                   ))
            {
                while (reader.Read())
                {
                    output = DataReader(reader);
                }
            };
            return output;
        }

        internal async Task<decimal> GetPriceForAllBooksByCurrencyAsync(Enumeration.Currency currency)
        {
            _parameters = new List<SqlParameter>
            {
                new SqlParameter("Currency", currency.ToString())
            };

            var output = await DatabaseManager.ExecuteScalarAsync<decimal>(
                       StoredProcedure.GetPriceForAllBooksByCurrency,
                       _parameters,
                       ConnectionString
                   );

            return output;
        }

        internal async Task<List<Model>> GetPricesAsync()
        {
            var output = new List<Model>();
            using (var reader = await DatabaseManager.ExecuteReaderAsync(
                       StoredProcedure.GetPrices,
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
                BookId = reader.GetValue<Guid>(Field.BookId),
                Currency = Enum.Parse<Enumeration.Currency>(reader.GetValue<string>(Field.Currency)),
                Value = reader.GetValue<decimal>(Field.Value),
            };

            return output;
        }

        Model ITable<Model>.DataReader(IDataReader reader)
        {
            return DataReader(reader);
        }
    }
}
