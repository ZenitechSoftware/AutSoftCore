using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AutSoft.AspNetCore.Blazor.ComponentState;

/// <summary>
/// Base class for stateful components.
/// </summary>
public class StatefulComponentBase : ComponentBase, IDisposable
{
    private string? _instanceKey;
    private bool _disposed;

    /// <summary>
    /// Indicates that the component state should be saved.
    /// </summary>
    protected virtual bool ShouldSaveState { get; } = true;

    private IComponentStateStorage ComponentStateStorage { get; set; } = null!;

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
        if (_disposed)
            return;

        if (disposing && !string.IsNullOrEmpty(_instanceKey))
            ComponentStateStorage.SaveStateForComponent(_instanceKey, this);

        _disposed = true;
    }
}
