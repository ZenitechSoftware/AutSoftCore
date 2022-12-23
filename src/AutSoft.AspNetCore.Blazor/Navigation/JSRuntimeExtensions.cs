using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AutSoft.AspNetCore.Blazor.Navigation;

/// <summary>
/// Navigation extension methods for <see cref="IJSRuntime">IJSRuntime</see>.
/// </summary>
public static class JSRuntimeExtensions
{
    private static readonly NavigationManager NavigationManager = default!;

    /// <summary>
    /// Navigate back.
    /// </summary>
    public static ValueTask GoBackAsync(this IJSRuntime jSRuntime) =>
        jSRuntime.InvokeVoidAsync("NavigationHistoryManager.Instance.GoBack");

    /// <summary>
    /// Navigate back.
    /// </summary>
    public static ValueTask GoBackAsync(this IJSRuntime jSRuntime, object parameter)
    {
        NavigationManager.AddGoBackParameter(parameter);
        return jSRuntime.InvokeVoidAsync("NavigationHistoryManager.Instance.GoBack");
    }

    /// <summary>
    /// Back navigation possible.
    /// </summary>
    public static ValueTask<bool> CanGoBackAsync(this IJSRuntime jSRuntime)
    {
        return jSRuntime.InvokeAsync<bool>("is_nav_backward_possible");
    }
}
