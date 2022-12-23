using MudBlazor;

namespace AutSoft.AspNetCore.Blazor.Dialog.Confirm;

/// <summary>
/// Confirmation dialog extension methods for <see cref="IDialogService">IDialogService</see>.
/// </summary>
public static class ConfirmDialogExtensions
{
    /// <summary>
    /// Show confirmation dialog.
    /// </summary>
    public static async Task<bool> ShowConfirmDialog(this IDialogService dialogService, string title, string description) =>
        await dialogService.ShowDialogResultAsync<ConfirmDialog, ConfirmDialogParameters, bool>(new ConfirmDialogParameters(title, description));
}
