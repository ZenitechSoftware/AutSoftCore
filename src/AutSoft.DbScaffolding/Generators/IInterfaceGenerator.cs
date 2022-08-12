using Microsoft.EntityFrameworkCore.Metadata;

namespace AutSoft.DbScaffolding.Generators;

internal interface IInterfaceGenerator
{
    string GenerateSoftDeletableInterface(IEntityType entityType, bool useNullableReferenceTypes);
    string GenerateAuditableInterface(IEntityType entityType, bool useNullableReferenceTypes);
}
