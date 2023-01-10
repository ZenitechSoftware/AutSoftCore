using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AutSoft.AspNetCore.Blazor.ComponentState;

/// <summary>
/// Base class for stateful components.
/// </summary>
public class StatefulComponentBase : ComponentBase, IDisposable
{
    private string? _instanceKey;
    private bool _isDisposed;

    /// <summary>
    /// Indicates that the component state should be saved.
    /// </summary>
    protected virtual bool ShouldSaveState { get; } = true;

    [Inject]
    private IComponentStateStorage ComponentStateStorage { get; set; } = null!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (ShouldSaveState)
        {
            _instanceKey = await JSRuntime.GetNavStateTimeKeyAsync();
            ComponentStateStorage.RestoreStateForComponent(_instanceKey, this);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing && !string.IsNullOrEmpty(_instanceKey))
        {
            ComponentStateStorage.SaveStateForComponent(_instanceKey, this);
        }

        _isDisposed = true;
    }
}
