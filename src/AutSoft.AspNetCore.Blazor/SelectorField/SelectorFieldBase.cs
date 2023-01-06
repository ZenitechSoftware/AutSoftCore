using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using MudBlazor;

namespace AutSoft.AspNetCore.Blazor.SelectorField;

/// <summary>
/// Selector field base.
/// </summary>
/// <typeparam name="TItem">Item type.</typeparam>
public abstract class SelectorFieldBase<TItem> : MudTextField<TItem>
{
    /// <summary>
    /// Dialog service.
    /// </summary>
    [Inject]
    protected IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Constructor of the SelectorFieldBase
    /// </summary>
    protected SelectorFieldBase()
    {
        Adornment = Adornment.End;
        ReadOnly = true;
        AdornmentIcon = Icons.Material.Filled.Edit;
        OnAdornmentClick = new EventCallback<MouseEventArgs>(this, OnEditAsync);
    }

    private async Task OnEditAsync()
    {
        var item = await SelectItemAsync();
        if (EqualityComparer<TItem>.Default.Equals(item, default))
            return;

        await SetValueAsync(item!);
        StateHasChanged();
    }

    /// <summary>
    /// Called when the adornment icon is clicked and returns with the selected item.
    /// </summary>
    /// <returns>Selected item.</returns>
    protected abstract Task<TItem?> SelectItemAsync();
}
