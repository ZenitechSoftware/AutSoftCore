using AutSoft.AspNetCore.Blazor.ErrorHandling;

using MudBlazor;

namespace AutSoft.AspNetCore.Blazor.Loading;

/// <summary>
/// Custom action loading error handler.
/// </summary>
public class CustomActionLoadingErrorHandler : DefaultLoadingErrorHandler
{
    private readonly Func<Exception, Task<bool>> _handler;

    /// <summary>
    /// Constructor of the CustomActionLoadingErrorHandler.
    /// </summary>
    /// <param name="handler">Exception handler function.</param>
    /// <param name="displayErrorFactory">Display error factory.</param>
    /// <param name="dialogService">Dialog service.</param>
    public CustomActionLoadingErrorHandler(Func<Exception, Task<bool>> handler, IDisplayErrorFactory displayErrorFactory, IDialogService dialogService)
        : base(displayErrorFactory, dialogService)
    {
        _handler = handler;
    }

    /// <inheritdoc />
    public override async Task HandleErrorAsync(LoadingOperation loadingOperation, Exception exception)
    {
        if (await _handler(exception))
            return;

        await base.HandleErrorAsync(loadingOperation, exception);
    }
}
