using AutSoft.Common.Time;

using Microsoft.Extensions.DependencyInjection;

namespace AutSoft.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutSoftCommon(this IServiceCollection services)
    {
        return services.AddSingleton<ITimeProvider, TimeProvider>();
    }
}
