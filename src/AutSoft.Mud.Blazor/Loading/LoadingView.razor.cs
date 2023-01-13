using Microsoft.AspNetCore.Components;

using MudBlazor.Utilities;

namespace AutSoft.Mud.Blazor.Loading;

/// <summary>
/// Loading view.
/// </summary>
public partial class LoadingView
{
    private LoadingOperation? _loadingOperation;

    /// <summary>
    /// Child content to display.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Render content while loading.
    /// </summary>
    [Parameter]
    public bool RenderContentWhileLoading { get; set; }

    /// <summary>
    /// Loading operation.
    /// </summary>
    [Parameter]
    public LoadingOperation? LoadingOperation
    {
        get => _loadingOperation;
        set
        {
            if (_loadingOperation != null)
            {
                _loadingOperation.StateChanged -= OnLoadingStateChanged;
                _loadingOperation.BlockingChanged -= OnBlockingStateChanged;
            }

            _loadingOperation = value;
            _loadingOperation!.StateChanged += OnLoadingStateChanged;
            _loadingOperation!.BlockingChanged += OnBlockingStateChanged;
        }
    }

    /// <summary>
    /// Style used to format the item.
    /// </summary>
    protected string Stylename =>
        StyleBuilder.Empty()
            .AddStyle(Style)
            .Build();

    /// <summary>
    /// Class used to format the item.
    /// </summary>
    protected string Classname =>
        CssBuilder.Default("loading-view-root")
            .AddClass(Class)
            .Build();

    /// <summary>
    /// Class used to format container.
    /// </summary>
    protected string ContainerClassname =>
        CssBuilder.Empty()
            .AddClass(Class)
            .AddClass("d-none", !LoadingOperation?.IsContentVisible)
            .Build();

    private void OnLoadingStateChanged(object? sender, LoadingStateChangedEventArgs e) => StateHasChanged();

    private void OnBlockingStateChanged(object? sender, BlockingStateChangedEventArgs e) => StateHasChanged();

    private void OnRetry() => LoadingOperation?.RetryAsync();
}
