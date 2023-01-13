using AutSoft.AspNetCore.Blazor.Enumeration;

using Microsoft.AspNetCore.Components;

using MudBlazor;

namespace AutSoft.Mud.Blazor.Enumeration;

/// <summary>
/// Enumeration autocomplete.
/// </summary>
/// <typeparam name="T">Enumeration type.</typeparam>
/// <typeparam name="TEnum">Collection of the enumeration types.</typeparam>
public class EnumerationAutocomplete<T, TEnum> : MudAutocomplete<T> where TEnum : Enum
{
    /// <summary>
    /// Items number to show.
    /// </summary>
    [Parameter]
    public int Take { get; set; } = 10;

    /// <summary>
    /// Enumeration type.
    /// </summary>
    [Parameter]
    public TEnum Type { get; set; } = default!;

    [Inject]
    private IEnumerationCache<TEnum> EnumerationCache { get; set; } = null!;

    private List<EnumerationItem> _enumerationItems = new();

    /// <summary>
    /// Default constructor of the EnumerationAutocomplete.
    /// </summary>
    public EnumerationAutocomplete()
    {
        var enumType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
        if (!enumType.IsEnum && enumType != typeof(int))
            throw new NotSupportedException("Only enum/int or nullable enum/int types are supported!");

        ToStringFunc = (t) => t == null ? null : _enumerationItems.SingleOrDefault(d => d.Id == (int)(object)t!)?.DisplayName;

        SearchFunc = (filter) => Task.FromResult(
            _enumerationItems
            .Where(c => string.IsNullOrEmpty(filter) || c.DisplayName.Contains(filter, StringComparison.InvariantCultureIgnoreCase))
            .Take(Take)
            .Select(c => enumType.IsEnum ? (T)Enum.ToObject(enumType, c.Id) : (T)(object)c.Id));
    }

    /// <inheritdoc />
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (!Type.Equals(default(TEnum)))
        {
            _enumerationItems = await EnumerationCache.ResolveEnumerationItemsAsync(Type);
            await UpdateTextPropertyAsync(false);
            StateHasChanged();
        }
    }
}
