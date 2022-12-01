using AutSoft.Common.Time;

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
        return services.AddSingleton<ITimeProvider, TimeProvider>();
    }
}
