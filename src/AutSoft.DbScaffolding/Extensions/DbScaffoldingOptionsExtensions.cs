using NetTopologySuite.Geometries;
using System;

namespace AutSoft.DbScaffolding.Extensions
{
    public static class DbScaffoldingOptionsExtensions
    {
        public static TableBuilder UseTable(this DbScaffoldingOptions options, string tableName, string schemaName = "dbo")
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name is not provided.", nameof(tableName));
            }
            return new TableBuilder
            {
                Options = options,
                SchemaName = schemaName,
                TableName = tableName
            };
        }

        public static TableBuilder UseTable(this TableBuilder tableBuilder, string tableName, string schemaName = "dbo")
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name is not provided.", nameof(tableName));
            }
            tableBuilder.TableName = tableName;
            tableBuilder.SchemaName = schemaName;
            return tableBuilder;
        }

        public static TableBuilder AddEnumColumn<TEnum>(this TableBuilder tableBuilder, string columnName)
            where TEnum : Enum
        {
            if (string.IsNullOrEmpty(columnName))
            {
                throw new ArgumentException("Column name is not provided.", nameof(columnName));
            }

            tableBuilder.Options.ColumnToEnumDictionary.Add(new DbColumn(tableBuilder.SchemaName, tableBuilder.TableName, columnName), typeof(TEnum));

            return tableBuilder;
        }

        public static TableBuilder AddSpatialTypeColumn<TSpatial>(this TableBuilder tableBuilder, string columnName)
            where TSpatial : Geometry
        {
            if (string.IsNullOrEmpty(columnName))
            {
                throw new ArgumentException("Column name is not provided.", nameof(columnName));
            }

            tableBuilder.Options.ColumnToSpatialTypeDictionary.Add(new DbColumn(tableBuilder.SchemaName, tableBuilder.TableName, columnName), typeof(TSpatial));

            return tableBuilder;
        }
    }
}
