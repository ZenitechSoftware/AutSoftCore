using Microsoft.EntityFrameworkCore.Metadata;

using System.Linq;

namespace AutSoft.DbScaffolding;

public static class EntityTypeExtensions
{
    public static bool IsAuditable(this IEntityType entityType, DbScaffoldingOptions dbScaffoldingOptions)
    {
        return entityType.GetProperties().Any(p => p.Name == dbScaffoldingOptions.InterfaceProperties.LastModAt);
    }

    public static bool IsDeletable(this IEntityType entityType, DbScaffoldingOptions dbScaffoldingOptions)
    {
        return entityType.GetProperties().Any(p => p.Name == dbScaffoldingOptions.InterfaceProperties.IsDeleted);
    }
}
