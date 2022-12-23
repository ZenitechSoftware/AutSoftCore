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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "Base service")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Base service")]
    protected readonly IDisplayErrorFactory _displayErrorFactory;

    /// <summary>
    /// Dialog service.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "Base service")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Base service")]
    protected readonly IDialogService _dialogService;

    /// <summary>
    /// Constructor of the DefaultLoadingErrorHandler.
    /// </summary>
    /// <param name="displayErrorFactory">Display error factory.</param>
    /// <param name="dialogService">Dialog service.</param>
    public DefaultLoadingErrorHandler(IDisplayErrorFactory displayErrorFactory, IDialogService dialogService)
    {
        _displayErrorFactory = displayErrorFactory;
        _dialogService = dialogService;
    }

    /// <inheritdoc />
    public virtual Task HandleErrorAsync(LoadingOperation loadingOperation, Exception exception)
    {
        if (exception is TaskCanceledException)
            return Task.CompletedTask;

        var displayError = _displayErrorFactory.CreateDisplayError(exception);

        return HandleErrorCoreAsync(loadingOperation, displayError);
    }

    /// <summary>
    /// If overridden, can contain specific logic for error handling.
    /// </summary>
    protected virtual async Task HandleErrorCoreAsync(LoadingOperation loadingOperation, DisplayError error)
    {
        if (loadingOperation.IsBlocking)
            loadingOperation.Failed(error);
        else
        {
            loadingOperation.Done();
            await _dialogService.ShowErrorDialogAsync(error, loadingOperation.CopyDetailsAsync);
        }
    }
}
