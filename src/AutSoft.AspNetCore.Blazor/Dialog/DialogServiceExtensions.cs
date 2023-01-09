using MudBlazor;

namespace AutSoft.AspNetCore.Blazor.Dialog;

/// <summary>
/// Extension methods for <see cref="IDialogService">IDialogService</see>.
/// </summary>
public static class DialogServiceExtensions
{
    /// <summary>
    /// Show dialog.
    /// </summary>
    /// <typeparam name="TDialog">Dialog type to show.</typeparam>
    public static Task ShowDialogAsync<TDialog>(this IDialogService dialogService)
        where TDialog : DialogComponentBase =>
        dialogService.Show<TDialog>(string.Empty).Result;

    /// <summary>
    /// Show dialog with parameters.
    /// </summary>
    /// <typeparam name="TDialog">Dialog type to show.</typeparam>
    /// <typeparam name="TParameter">Parameter type.</typeparam>
    public static Task ShowDialogAsync<TDialog, TParameter>(this IDialogService dialogService, TParameter parameter, DialogOptions? dialogOptions = null)
        where TDialog : DialogComponentBaseTParameter<TParameter> =>
        dialogService.Show<TDialog>(
            string.Empty,
            new DialogParameters
            {
                [nameof(DialogComponentBaseTParameter<TParameter>.Parameter)] = parameter,
            },
            dialogOptions).Result;

    /// <summary>
    /// Show dialog with parameter and result.
    /// </summary>
    /// <typeparam name="TDialog">Dialog type to show.</typeparam>
    /// <typeparam name="TParameter">Parameter type.</typeparam>
    /// <typeparam name="TResult">Result type.</typeparam>
    public static async Task<TResult> ShowDialogResultAsync<TDialog, TParameter, TResult>(this IDialogService dialogService, TParameter parameter, DialogOptions? dialogOptions = null)
        where TDialog : DialogComponentBaseTParameterTResult<TParameter, TResult>
    {
        var result = await dialogService.Show<TDialog>(
            string.Empty,
            new DialogParameters
            {
                [nameof(DialogComponentBaseTParameter<TParameter>.Parameter)] = parameter,
            },
            dialogOptions).Result;

        return (TResult)result.Data;
    }

    /// <summary>
    /// Show dialog with result.
    /// </summary>
    /// <typeparam name="TDialog">Dialog type to show.</typeparam>
    /// <typeparam name="TResult">Result type</typeparam>
    public static async Task<TResult> ShowDialogResultAsync<TDialog, TResult>(this IDialogService dialogService, DialogOptions? dialogOptions = null)
        where TDialog : DialogComponentBaseTResult<TResult>
    {
        var result = await dialogService.Show<TDialog>(string.Empty, dialogOptions).Result;
        return (TResult)result.Data;
    }
}
