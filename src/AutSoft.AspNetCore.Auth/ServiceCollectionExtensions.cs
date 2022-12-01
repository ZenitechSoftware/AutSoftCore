using AutSoft.AspNetCore.Auth.AnyPolicies;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AutSoft.AspNetCore.Auth;

/// <summary>
/// Register all relevant services into dependency injection container
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register all relevant services into dependency injection container
    /// </summary>
    /// <returns>Expanded service collection</returns>
    public static IServiceCollection AddAutSoftAspNetCoreAuth(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationPolicyProvider, AnyPoliciesPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, AnyPoliciesAuthorizationHandler>();

        return services;
    }
}
