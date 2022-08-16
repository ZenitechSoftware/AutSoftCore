using Microsoft.EntityFrameworkCore.Metadata;

using System;
using System.Linq;

namespace AutSoft.DbScaffolding.Identity.Extensions;

internal static class ModelExtensions
{
    public static Type FindIdentityKeyType(this IModel model, string userTableName)
    {
        var userEntityType = model.GetEntityTypes().Single(e => e.Name == userTableName);
        var userKey = userEntityType.FindPrimaryKey();

        if (userKey.Properties.Count != 1)
            throw new NotSupportedException("Composite keys are not supported on identity tables!");

        return userKey.Properties.Single().ClrType;
    }
}
