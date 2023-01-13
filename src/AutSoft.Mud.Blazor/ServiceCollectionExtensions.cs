using AutSoft.Mud.Blazor.ErrorHandling;
using AutSoft.Mud.Blazor.Loading;

using Microsoft.Extensions.DependencyInjection;

using MudBlazor.Services;

namespace AutSoft.Mud.Blazor;

/// <summary>
/// Register all relevant services into dependency injection container
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register all relevant services into dependency injection container
    /// </summary>
    /// <returns>Expanded service collection</returns>
    public static IServiceCollection AddAutSoftMudBlazor(this IServiceCollection services)
    {
        services.AddMudServices();
        services.AddScoped<CustomActionLoadingErrorHandlerFactory>();
        services.AddScoped<DefaultLoadingErrorHandlerFactory>();
        services.AddTransient<LoadingOperation>();
        services.AddTransient<BlockingLoadingOperation>();

        return services;
    }
}
