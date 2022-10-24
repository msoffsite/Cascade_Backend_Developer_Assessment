using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CascadeFinTech.Data.Extensions
{
    internal static class DataReader
    {
        internal static T GetValue<T>(this IDataReader reader, string columnName)
        {
            return reader.GetValue(columnName, default(T));
        }

        internal static T GetValue<T>(this IDataReader reader, string columnName, T defaultValue)
        {
            T value = defaultValue;
            var columnValue = reader[columnName];
            if (columnValue != DBNull.Value)
            {
                value = columnValue.ConvertValue<T>();
            }
            return value;
        }
    }
}
