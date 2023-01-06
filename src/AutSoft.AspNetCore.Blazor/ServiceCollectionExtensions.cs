using AutSoft.AspNetCore.Blazor.Clipboard;
using AutSoft.AspNetCore.Blazor.ComponentState;
using AutSoft.AspNetCore.Blazor.ErrorHandling;
using AutSoft.AspNetCore.Blazor.Loading;
using AutSoft.AspNetCore.Blazor.Localization;

using Microsoft.Extensions.DependencyInjection;

using MudBlazor.Services;

namespace AutSoft.AspNetCore.Blazor;

/// <summary>
/// Register all relevant services into dependency injection container
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register all relevant services into dependency injection container
    /// </summary>
    /// <returns>Expanded service collection</returns>
    public static IServiceCollection AddAutSoftAspNetCoreBlazor(this IServiceCollection services)
    {
        services.AddMudServices();
        services.AddScoped<IClipboardService, ClipboardService>();
        services.AddSingleton<IComponentStateStorage, ComponentStateStorage>();
        services.AddScoped<IDisplayErrorFactory, DisplayErrorFactory>();
        services.AddScoped<CustomActionLoadingErrorHandlerFactory>();
        services.AddScoped<DefaultLoadingErrorHandlerFactory>();
        services.AddTransient<LoadingOperation>();
        services.AddTransient<BlockingLoadingOperation>();
        services.AddSingleton<BlazorComponentLocalizer>();

        return services;
    }
}
