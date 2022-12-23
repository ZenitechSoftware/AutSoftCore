using AutSoft.AspNetCore.Auth;
using AutSoft.AspNetCore.Blazor;
using AutSoft.Common;

using Microsoft.Extensions.DependencyInjection;

namespace AutSoft.All;

/// <summary>
/// Register all relevant services into dependency injection container
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register all relevant services into dependency injection container
    /// </summary>
    /// <returns>Expanded service collection</returns>
    public static IServiceCollection AddAutSoftAll(this IServiceCollection services)
    {
        services.AddAutSoftAspNetCoreBlazor();
        services.AddAutSoftAspNetCoreAuth();
        services.AddAutSoftCommon();

        return services;
    }
}
