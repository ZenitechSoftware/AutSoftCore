using AutSoft.AspNetCore.Blazor.Clipboard;
using AutSoft.AspNetCore.Blazor.ComponentState;
using AutSoft.AspNetCore.Blazor.ErrorHandling;
using AutSoft.AspNetCore.Blazor.Localization;

using Microsoft.Extensions.DependencyInjection;

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
        services.AddScoped<IDisplayErrorFactory, DisplayErrorFactory>();
        services.AddScoped<IClipboardService, ClipboardService>();
        services.AddSingleton<IComponentStateStorage, ComponentStateStorage>();
        services.AddSingleton<BlazorComponentLocalizer>();

        return services;
    }
}
