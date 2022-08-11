using AutSoft.DbScaffolding.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AutSoft.DbScaffolding.Identity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIdentityScaffolding(
            this IServiceCollection services,
            Action<IdentityScaffoldingOptions> identityConfigureOptions)
        {
            services.AddDatabaseScaffoldingCore<IdentityCSharpEntityTypeGenerator, IdentityCSharpDbContextGenerator>((options) =>
            {
                identityConfigureOptions(new IdentityScaffoldingOptions
                {
                    DbScaffoldingOptions = options
                });
            });

            services.Configure(identityConfigureOptions);
        }
    }
}
