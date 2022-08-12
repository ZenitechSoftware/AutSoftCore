using AutSoft.DbScaffolding.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using System.Linq;

namespace AutSoft.DbScaffolding.Helpers;

internal static class EntityTypeExtensions
{
    public static bool IsAuditable(this IEntityType entityType, DbScaffoldingOptions dbScaffoldingOptions)
    {
        return entityType.GetProperties().Any(p => p.Name == dbScaffoldingOptions.InterfaceProperties.LastModAt);
    }

    public static bool IsDeletable(this IEntityType entityType, DbScaffoldingOptions dbScaffoldingOptions)
    {
        return entityType.GetProperties().Any(p => p.Name == dbScaffoldingOptions.InterfaceProperties.IsDeleted);
    }

    public static string GetSchemaName(this IEntityType entityType)
    {
        return !string.IsNullOrEmpty(entityType.GetTableName())
            ? entityType.GetSchema()
            : entityType.GetViewSchema();
    }
}
