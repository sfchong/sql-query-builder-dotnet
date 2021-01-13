using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SqlQueryBulders.Core
{
    public static class SqlQueryBulder
    {
        public static string BuildInsertQuery<T>()
        {
            string tableName = typeof(T).Name;
            List<string> columnList = PropertyUtility.GetPropertyNames<T>();

            List<string> columnListWithSquareBracket = columnList.Select(col => $"[{col}]").ToList();
            string columnString = string.Join(",", columnListWithSquareBracket);

            List<string> paramList = columnList.Select(s => $"@{s}").ToList();
            string paramString = string.Join(",", paramList);

            return $"DECLARE @t TABLE([Id] UNIQUEIDENTIFIER); INSERT INTO [{tableName}] ({columnString}) OUTPUT INSERTED.[Id] INTO @t VALUES ({paramString}); SELECT [Id] FROM @t ";
        }

        public static string BuildSelectAllQuery<T>()
        {
            string tableName = typeof(T).Name;

            List<string> columnList = PropertyUtility.GetPropertyNames<T>();
            columnList = columnList.Select(colName => $"[{colName}]").ToList();
            string columnString = string.Join(",", columnList);

            string query = $@"SELECT {columnString} FROM [{tableName}] WHERE 1=1 ";

            return query;
        }

        public static string BuildSelectQuery<T>(Expression<Func<T, object>> properties, Expression<Func<T, object>> parameters = null)
        {
            string tableName = typeof(T).Name;
            var columnList = PropertyUtility.GetPropertyNames(properties).Select(name => $"[{name}]");
            var paramList = PropertyUtility.GetPropertyNames(parameters)?.Select(name => $"[{name}]=@{name}");
            StringBuilder sqlString = new StringBuilder();

            // Construct and append column string
            string columnString = string.Join(",", columnList);
            sqlString.Append($@"SELECT {columnString} FROM [{tableName}] WHERE 1=1 ");

            // Construct and append parameter string
            if (paramList != null && paramList.Any())
            {
                sqlString.Append(" AND ");
                string paramString = string.Join(" AND ", paramList);
                sqlString.Append(paramString);
            }

            return sqlString.ToString();
        }

        public static string BuildUpdateQuery<T>(Expression<Func<T, object>> propertiesToUpdate = null)
        {
            string tableName = typeof(T).Name;

            List<string> columnList = (propertiesToUpdate != null) 
                ? PropertyUtility.GetPropertyNames(propertiesToUpdate)
                : PropertyUtility.GetPropertyNames<T>();

            List<string> paramList = columnList.Select(name => $"[{name}]=@{name}").ToList();
            string paramString = string.Join(",", paramList);

            return $"UPDATE [{tableName}] SET {paramString} WHERE 1=1 ";
        }
    }
}
