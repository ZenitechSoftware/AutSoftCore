namespace AutSoft.DbScaffolding
{
    public class DbColumn
    {
        public DbColumn(string schemaName, string tableName, string columnName)
        {
            SchemaName = schemaName;
            TableName = tableName;
            ColumnName = columnName;
        }

        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is DbColumn dbColumn)
            {
                return dbColumn.SchemaName == SchemaName
                    && dbColumn.TableName == TableName
                    && dbColumn.ColumnName == ColumnName;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return SchemaName.GetHashCode() ^ TableName.GetHashCode() ^ ColumnName.GetHashCode();
        }
    }
}
