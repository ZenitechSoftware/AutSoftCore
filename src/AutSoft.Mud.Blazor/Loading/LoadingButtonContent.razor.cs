using Microsoft.AspNetCore.Components;

namespace AutSoft.Mud.Blazor.Loading;

/// <summary>
/// Loading button content.
/// </summary>
public partial class LoadingButtonContent
{
    private LoadingOperation? _loadingOperation;

    /// <summary>
    /// Child content to display.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Loadnig operation.
    /// </summary>
    [Parameter]
    public LoadingOperation? LoadingOperation
    {
        get => _loadingOperation;
        set
        {
            if (_loadingOperation != null)
                _loadingOperation.StateChanged -= OnLoadingStateChanged;

            _loadingOperation = value;
            _loadingOperation!.StateChanged += OnLoadingStateChanged;
        }
    }

    private void OnLoadingStateChanged(object? sender, LoadingStateChangedEventArgs e) => StateHasChanged();
}
