using AutSoft.Common;

using Microsoft.Extensions.DependencyInjection;

namespace AutSoft.All;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutSoftAll(this IServiceCollection services)
    {
        return services.AddAutSoftCommon();
    }
}
