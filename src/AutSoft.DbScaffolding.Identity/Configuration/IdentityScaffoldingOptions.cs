using AutSoft.DbScaffolding.Configuration;

using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;

namespace AutSoft.DbScaffolding.Identity.Configuration;

/// <summary>
/// Configuration of scaffolding code first model with ASP.NET Core Identity
/// </summary>
public class IdentityScaffoldingOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityScaffoldingOptions"/> class.
    /// </summary>
    public IdentityScaffoldingOptions()
    {
        UserTableName = "User";
        RoleTableName = "Role";
        RoleClaimTableName = "RoleClaim";
        UserClaimTableName = "UserClaim";
        UserLoginTableName = "UserLogin";
        UserRoleTableName = "UserRole";
        UserTokenTableName = "UserToken";
    }

    /// <summary>
    /// Gets or sets name of identity user table
    /// </summary>
    public string UserTableName { get; set; }

    /// <summary>
    /// Gets or sets name of identity role table
    /// </summary>
    public string RoleTableName { get; set; }

    /// <summary>
    /// Gets or sets name of identity role-claim cross reference table
    /// </summary>
    public string RoleClaimTableName { get; set; }

    /// <summary>
    /// Gets or sets name of identity user-claim cross reference table
    /// </summary>
    public string UserClaimTableName { get; set; }

    /// <summary>
    /// Gets or sets name of identity user login table
    /// </summary>
    public string UserLoginTableName { get; set; }

    /// <summary>
    /// Gets or sets name of identity user-role cross reference table
    /// </summary>
    public string UserRoleTableName { get; set; }

    /// <summary>
    /// Gets or sets name of identity user token table
    /// </summary>
    public string UserTokenTableName { get; set; }

    /// <summary>
    /// Gets or sets base scaffolding configuration
    /// </summary>
    public DbScaffoldingOptions DbScaffoldingOptions { get; set; } = new DbScaffoldingOptions();

    internal Dictionary<string, Type> CreateIdentityTableNameLookup(Type keyType)
    {
        return new Dictionary<string, Type>
        {
            { UserTableName, typeof(IdentityUser<>).MakeGenericType(keyType) },
            { RoleTableName, typeof(IdentityRole<>).MakeGenericType(keyType) },
            { RoleClaimTableName, typeof(IdentityRoleClaim<>).MakeGenericType(keyType) },
            { UserClaimTableName, typeof(IdentityUserClaim<>).MakeGenericType(keyType) },
            { UserLoginTableName, typeof(IdentityUserLogin<>).MakeGenericType(keyType) },
            { UserRoleTableName, typeof(IdentityUserRole<>).MakeGenericType(keyType) },
            { UserTokenTableName, typeof(IdentityUserToken<>).MakeGenericType(keyType) },
        };
    }
}
