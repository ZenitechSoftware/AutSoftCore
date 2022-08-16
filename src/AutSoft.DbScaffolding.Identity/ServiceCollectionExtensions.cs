using AutSoft.DbScaffolding.Identity.Configuration;
using AutSoft.DbScaffolding.Identity.Generators;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace AutSoft.DbScaffolding.Identity;

/// <summary>
/// Register all relevant services into dependency injection container
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register all relevant services into dependency injection container
    /// </summary>
    public static IServiceCollection AddIdentityScaffolding(this IServiceCollection services, Action<IdentityScaffoldingOptions> identityConfigureOptions)
    {
        services.AddDatabaseScaffoldingCore<IdentityCSharpEntityTypeGenerator, IdentityCSharpDbContextGenerator>(
            options => identityConfigureOptions(new IdentityScaffoldingOptions
            {
                DbScaffoldingOptions = options,
            }));

        services.Configure(identityConfigureOptions);

        return services;
    }
}
