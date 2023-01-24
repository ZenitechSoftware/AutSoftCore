using AutSoft.AspNetCore.Blazor.Enumeration;

using Microsoft.AspNetCore.Components;

using MudBlazor;

namespace AutSoft.Mud.Blazor.Enumeration;

/// <summary>
/// Enumeration display.
/// </summary>
/// <typeparam name="TEnum">Collection of the enumeration types.</typeparam>
public partial class EnumerationDisplay<TEnum> : MudComponentBase where TEnum : Enum
{
    /// <summary>
    /// Enumeration type.
    /// </summary>
    [Parameter]
    public TEnum Type { get; set; } = default!;

    /// <summary>
    /// Enumeration type id.
    /// </summary>
    [Parameter]
    public int? Id { get; set; }

    [Inject]
    private IEnumerationCache<TEnum> EnumerationCache { get; set; } = null!;

    private string? _displayName;

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        _displayName = !Type.Equals(default(TEnum)) && Id.HasValue && Id.Value != default
            ? await EnumerationCache.ResolveDisplayNameAsync(Type, Id.Value)
            : null;
    }
}
