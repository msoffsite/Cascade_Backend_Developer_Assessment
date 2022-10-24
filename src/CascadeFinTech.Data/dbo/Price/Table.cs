using CascadeFinTech.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
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
                new SqlParameter("Currency", currency)
            };

            Model result = null;
            using (var reader = await DatabaseManager.ExecuteReaderAsync(
                       StoredProcedure.GetPriceByBookIdCurrency,
                       _parameters,
                       ConnectionString
                   ).ConfigureAwait(false))
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
                BookId = reader.GetValue<Guid>(Field.BookId),
                Currency = reader.GetValue<Enumeration.Currency>(Field.Currency),
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
