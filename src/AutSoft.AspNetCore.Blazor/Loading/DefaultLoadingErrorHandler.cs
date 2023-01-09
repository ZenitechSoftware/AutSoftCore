using AutSoft.AspNetCore.Blazor.ErrorHandling;

using MudBlazor;

namespace AutSoft.AspNetCore.Blazor.Loading;

/// <summary>
/// Default error handler.
/// </summary>
public class DefaultLoadingErrorHandler : ILoadingErrorHandler
{
    /// <summary>
    /// Display error factory.
    /// </summary>
    protected IDisplayErrorFactory DisplayErrorFactory { get; }

    /// <summary>
    /// Dialog service.
    /// </summary>
    protected IDialogService DialogService { get; }

    /// <summary>
    /// Constructor of the DefaultLoadingErrorHandler.
    /// </summary>
    /// <param name="displayErrorFactory">Display error factory.</param>
    /// <param name="dialogService">Dialog service.</param>
    public DefaultLoadingErrorHandler(IDisplayErrorFactory displayErrorFactory, IDialogService dialogService)
    {
        DisplayErrorFactory = displayErrorFactory;
        DialogService = dialogService;
    }

    /// <inheritdoc />
    public virtual Task HandleErrorAsync(LoadingOperation loadingOperation, Exception exception)
    {
        if (exception is TaskCanceledException)
            return Task.CompletedTask;

        var displayError = DisplayErrorFactory.CreateDisplayError(exception);

        return HandleErrorCoreAsync(loadingOperation, displayError);
    }

    /// <summary>
    /// If overridden, can contain specific logic for error handling.
    /// </summary>
    protected virtual async Task HandleErrorCoreAsync(LoadingOperation loadingOperation, DisplayError error)
    {
        if (loadingOperation.IsBlocking)
        {
            loadingOperation.Failed(error);
        }
        else
        {
            loadingOperation.Done();
            await DialogService.ShowErrorDialogAsync(error, loadingOperation.CopyDetailsAsync);
        }
    }
}
