using AutSoft.DbScaffolding.Configuration;

using NetTopologySuite.Geometries;

using System;

namespace AutSoft.DbScaffolding.Helpers;

/// <summary>
/// Extension methods to configure table builder
/// </summary>
public static class DbScaffoldingOptionsExtensions
{
    /// <summary>
    /// Creates a table builder
    /// </summary>
    /// <param name="options">Option to configure</param>
    /// <param name="tableName">Table's name to configure</param>
    /// <param name="schemaName">Table's containing schema, default is "dbo"</param>
    /// <returns>Table builder for table</returns>
    /// <exception cref="ArgumentException">Throws if table name is not provided</exception>
    public static TableBuilder UseTable(this DbScaffoldingOptions options, string tableName, string schemaName = "dbo")
    {
        if (string.IsNullOrEmpty(tableName))
            throw new ArgumentException("Table name is not provided.", nameof(tableName));

        return new TableBuilder
        {
            Options = options,
            SchemaName = schemaName,
            TableName = tableName,
        };
    }

    /// <summary>
    /// Configure a table builder
    /// </summary>
    /// <param name="tableBuilder">table builder to configure</param>
    /// <param name="tableName">Table's name to configure</param>
    /// <param name="schemaName">Table's containing schema, default is "dbo"</param>
    /// <returns>Table builder for table</returns>
    /// <exception cref="ArgumentException">Throws if table name is not provided</exception>
    public static TableBuilder UseTable(this TableBuilder tableBuilder, string tableName, string schemaName = "dbo")
    {
        if (string.IsNullOrEmpty(tableName))
            throw new ArgumentException("Table name is not provided.", nameof(tableName));

        tableBuilder.TableName = tableName;
        tableBuilder.SchemaName = schemaName;
        return tableBuilder;
    }

    /// <summary>
    /// Configure an enum column on a table
    /// </summary>
    /// <typeparam name="TEnum">Column's enum type</typeparam>
    /// <param name="tableBuilder">Table to configure</param>
    /// <param name="columnName">Column's name to configure</param>
    /// <returns>Table builder for table</returns>
    /// <exception cref="ArgumentException">Throws if column name is not provided</exception>
    public static TableBuilder AddEnumColumn<TEnum>(this TableBuilder tableBuilder, string columnName)
        where TEnum : Enum
    {
        if (string.IsNullOrEmpty(columnName))
            throw new ArgumentException("Column name is not provided.", nameof(columnName));

        tableBuilder.Options.ColumnToEnumDictionary.Add(new DbColumn(tableBuilder.SchemaName, tableBuilder.TableName, columnName), typeof(TEnum));

        return tableBuilder;
    }

    /// <summary>
    /// Configure an spatial column on a table
    /// </summary>
    /// <typeparam name="TSpatial">Column's spatial type</typeparam>
    /// <param name="tableBuilder">Table to configure</param>
    /// <param name="columnName">Column's name to configure</param>
    /// <returns>Table builder for table</returns>
    /// <exception cref="ArgumentException">Throws if column name is not provided</exception>
    public static TableBuilder AddSpatialTypeColumn<TSpatial>(this TableBuilder tableBuilder, string columnName)
        where TSpatial : Geometry
    {
        if (string.IsNullOrEmpty(columnName))
            throw new ArgumentException("Column name is not provided.", nameof(columnName));

        tableBuilder.Options.ColumnToSpatialTypeDictionary.Add(new DbColumn(tableBuilder.SchemaName, tableBuilder.TableName, columnName), typeof(TSpatial));

        return tableBuilder;
    }
}
