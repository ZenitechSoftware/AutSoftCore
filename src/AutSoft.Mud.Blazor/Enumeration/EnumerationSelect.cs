using AutSoft.AspNetCore.Blazor.Enumeration;

using Microsoft.AspNetCore.Components;

using MudBlazor;

using System.Globalization;

namespace AutSoft.Mud.Blazor.Enumeration;

/// <summary>
/// Enumeration select.
/// </summary>
/// <typeparam name="T">Enumeration type.</typeparam>
/// <typeparam name="TEnum">Collection of the enumeration types.</typeparam>
public class EnumerationSelect<T, TEnum> : MudSelect<T> where TEnum : Enum
{
    /// <summary>
    /// Enumeration type.
    /// </summary>
    [Parameter]
    public TEnum Type { get; set; } = default!;

    /// <summary>
    /// Enumeration type items filter callback.
    /// </summary>
    [Parameter]
    public Func<T, bool>? FilterFunc { get; set; }

    /// <summary>
    /// Disable select callback.
    /// </summary>
    [Parameter]
    public Func<T, bool>? DisabledFunc { get; set; }

    [Inject]
    private IEnumerationCache<TEnum> EnumerationCache { get; set; } = null!;

    private List<EnumerationItem> _enumerationItems = new();

    /// <summary>
    /// Default constructor of the EnumerationSelect.
    /// </summary>
    public EnumerationSelect()
    {
        ChildContent = (builder) =>
        {
            var enumType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            if (!enumType.IsEnum && enumType != typeof(int))
                throw new NotSupportedException("Only enum/int or nullable enum/int types are supported!");

            for (var i = 0; i < _enumerationItems.Count; i++)
            {
                var masterData = _enumerationItems[i];

                if (FilterFunc?.Invoke((T)(object)masterData.Id) == false)
                    continue;

                builder.OpenComponent<MudSelectItem<T>>(i);
                builder.AddAttribute(1, nameof(MudSelectItem<T>.Value), enumType.IsEnum ? Enum.ToObject(enumType, masterData.Id) : masterData.Id);
                builder.AddAttribute(2, nameof(MudSelectItem<T>.Disabled), DisabledFunc?.Invoke((T)(object)masterData.Id) ?? false);
                builder.AddContent(0, masterData.DisplayName);
                builder.CloseComponent();
            }
        };
        ToStringFunc = i => _enumerationItems.SingleOrDefault(m => m.Id == Convert.ToInt32(i, CultureInfo.InvariantCulture))?.DisplayName;
    }

    /// <inheritdoc />
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (Type.Equals(default(TEnum)))
        {
            _enumerationItems = await EnumerationCache.ResolveEnumerationItemsAsync(Type);
            await UpdateTextPropertyAsync(false);
            StateHasChanged();
        }
    }
}
