using AutSoft.Common.AnyPolicies;
using AutSoft.Common.Time;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AutSoft.Common;

/// <summary>
/// Register all relevant services into dependency injection container
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register all relevant services into dependency injection container
    /// </summary>
    /// <returns>Expanded service collection</returns>
    public static IServiceCollection AddAutSoftCommon(this IServiceCollection services)
    {
        services.AddSingleton<ITimeProvider, TimeProvider>();

        services.AddSingleton<IAuthorizationPolicyProvider, AnyPoliciesPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, AnyPoliciesAuthorizationHandler>();

        return services;
    }
}
