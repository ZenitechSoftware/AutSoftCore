using AutSoft.AspNetCore.Auth;
using AutSoft.Common;
using AutSoft.Mud.Blazor;

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
        services.AddAutSoftAspNetCoreAuth();
        services.AddAutSoftCommon();
        services.AddAutSoftMudBlazor();

        return services;
    }
}
