using AutSoft.AspNetCore.Blazor.ErrorHandling;

using Microsoft.Extensions.DependencyInjection;

using MudBlazor;

namespace AutSoft.AspNetCore.Blazor.Loading;

/// <summary>
/// Factory for <see cref="DefaultLoadingErrorHandler">Default loading error handler</see>.
/// </summary>
public class DefaultLoadingErrorHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Constructor of the DefaultLoadingErrorHandlerFactory.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    public DefaultLoadingErrorHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Creates the error handler.
    /// </summary>
    public ILoadingErrorHandler Create()
    {
        return new DefaultLoadingErrorHandler(
            _serviceProvider.GetRequiredService<IDisplayErrorFactory>(),
            _serviceProvider.GetRequiredService<IDialogService>()
        );
    }
}
