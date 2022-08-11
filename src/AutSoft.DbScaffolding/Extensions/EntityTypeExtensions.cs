using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AutSoft.DbScaffolding.Extensions;

public static class EntityTypeExtensions
{
    public static string GetSchemaName(this IEntityType entityType)
    {
        return !string.IsNullOrEmpty(entityType.GetTableName())
            ? entityType.GetSchema()
            : entityType.GetViewSchema();
    }
}
