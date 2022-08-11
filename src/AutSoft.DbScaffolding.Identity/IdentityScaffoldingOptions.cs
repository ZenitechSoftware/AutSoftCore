using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace AutSoft.DbScaffolding.Identity
{
    public class IdentityScaffoldingOptions
    {
        public string UserTableName { get; set; }
        public string RoleTableName { get; set; }
        public string RoleClaimTableName { get; set; }
        public string UserClaimTableName { get; set; }
        public string UserLoginTableName { get; set; }
        public string UserRoleTableName { get; set; }
        public string UserTokenTableName { get; set; }

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

        internal Dictionary<string, Type> CreateIdentityTableNameLookup(Type keyType)
        {
            return new Dictionary<string, Type>
            {
                {  UserTableName , typeof(IdentityUser<>).MakeGenericType(keyType) },
                {  RoleTableName , typeof(IdentityRole<>).MakeGenericType(keyType) },
                {  RoleClaimTableName ,typeof(IdentityRoleClaim<>).MakeGenericType( keyType) },
                {  UserClaimTableName , typeof(IdentityUserClaim<>).MakeGenericType(keyType) },
                {  UserLoginTableName , typeof(IdentityUserLogin<>).MakeGenericType(keyType) },
                {  UserRoleTableName , typeof(IdentityUserRole<>).MakeGenericType(keyType) },
                {  UserTokenTableName , typeof(IdentityUserToken<>).MakeGenericType(keyType) },
            };
        }

        public DbScaffoldingOptions DbScaffoldingOptions { get; set; } = new DbScaffoldingOptions();
    }
}
