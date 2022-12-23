using AutSoft.AspNetCore.Blazor.ErrorHandling;

using Microsoft.Extensions.DependencyInjection;

using MudBlazor;

namespace AutSoft.AspNetCore.Blazor.Loading;

/// <summary>
/// Factory for <see cref="CustomActionLoadingErrorHandler">Custom action loading error handler</see>.
/// </summary>
public class CustomActionLoadingErrorHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Constructor of the CustomActionLoadingErrorHandlerFactory.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    public CustomActionLoadingErrorHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Creates the error handler.
    /// </summary>
    public ILoadingErrorHandler Create(Func<Exception, Task<bool>> handler)
    {
        return new CustomActionLoadingErrorHandler(
            handler,
            _serviceProvider.GetRequiredService<IDisplayErrorFactory>(),
            _serviceProvider.GetRequiredService<IDialogService>()
        );
    }
}
