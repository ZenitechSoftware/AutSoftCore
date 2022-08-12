namespace AutSoft.DbScaffolding.Configuration;

/// <summary>
/// Data of a database column
/// </summary>
public class DbColumn
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbColumn"/> class.
    /// </summary>
    /// <param name="schemaName">Table's containing schema</param>
    /// <param name="tableName">Table's name</param>
    /// <param name="columnName">Column's name</param>
    public DbColumn(string schemaName, string tableName, string columnName)
    {
        SchemaName = schemaName;
        TableName = tableName;
        ColumnName = columnName;
    }

    /// <summary>
    /// Gets or sets table's containing schema
    /// </summary>
    public string SchemaName { get; set; }

    /// <summary>
    /// Gets or sets table's name
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// Gets or sets column's name
    /// </summary>
    public string ColumnName { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is DbColumn dbColumn)
        {
            return dbColumn.SchemaName == SchemaName
                && dbColumn.TableName == TableName
                && dbColumn.ColumnName == ColumnName;
        }

        return false;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return SchemaName.GetHashCode() ^ TableName.GetHashCode() ^ ColumnName.GetHashCode();
    }
}
