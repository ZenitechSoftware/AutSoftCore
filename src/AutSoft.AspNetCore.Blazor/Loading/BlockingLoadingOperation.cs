using AutSoft.AspNetCore.Blazor.Clipboard;
using AutSoft.AspNetCore.Blazor.ErrorHandling;

using Microsoft.Extensions.Logging;

using MudBlazor;

namespace AutSoft.AspNetCore.Blazor.Loading;

/// <summary>
/// Blocking state loading operation.
/// </summary>
public class BlockingLoadingOperation : LoadingOperation
{
    /// <summary>
    /// Constructor of the BlockingLoadingOperation.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="displayErrorFactory">Display error factory.</param>
    /// <param name="defaultLoadingErrorHandlerFactory">Default loading error handler factory.</param>
    /// <param name="clipboardService">Clipboard service.</param>
    /// <param name="snackbar">Snakbar.</param>
    public BlockingLoadingOperation(ILogger<LoadingOperation> logger, IDisplayErrorFactory displayErrorFactory, DefaultLoadingErrorHandlerFactory defaultLoadingErrorHandlerFactory, IClipboardService clipboardService, ISnackbar snackbar) : base(logger, displayErrorFactory, defaultLoadingErrorHandlerFactory, clipboardService, snackbar)
    {
        _isBlocking = true;
        _state = LoadingStateType.Loading;
    }
}
