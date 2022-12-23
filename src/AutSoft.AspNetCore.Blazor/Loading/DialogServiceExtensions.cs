using AutSoft.AspNetCore.Blazor.Dialog;
using AutSoft.AspNetCore.Blazor.ErrorHandling;

using MudBlazor;

namespace AutSoft.AspNetCore.Blazor.Loading;

/// <summary>
/// Error dialog extension methods for <see cref="IDialogService">IDialogService</see>.
/// </summary>
public static class DialogServiceExtensions
{
    /// <summary>
    /// Show error dialog.
    /// </summary>
    /// <param name="dialogService">Dialog service.</param>
    /// <param name="error">Error to display.</param>
    /// <param name="onCopyDetails">Callback used when copying details.</param>
    public static Task ShowErrorDialogAsync(this IDialogService dialogService, DisplayError error, Func<DisplayError, Task>? onCopyDetails = null) =>
        dialogService.ShowDialogAsync<ErrorDialog, ErrorDialogParameter>(new ErrorDialogParameter(error, onCopyDetails));
}
