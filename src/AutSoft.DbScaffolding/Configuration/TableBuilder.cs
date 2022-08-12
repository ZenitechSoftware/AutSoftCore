namespace AutSoft.DbScaffolding.Configuration;

/// <summary>
/// Builder to configure a SQL table's scaffolding
/// </summary>
public class TableBuilder
{
    internal string TableName { get; set; }
    internal string SchemaName { get; set; }
    internal DbScaffoldingOptions Options { get; set; }
}
