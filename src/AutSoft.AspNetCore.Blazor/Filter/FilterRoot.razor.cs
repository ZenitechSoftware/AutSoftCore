using AutSoft.AspNetCore.Blazor.Form;

using Microsoft.AspNetCore.Components;

using MudBlazor;
using MudBlazor.Utilities;

using System.Collections;

namespace AutSoft.AspNetCore.Blazor.Filter;

/// <summary>
/// Filter root.
/// </summary>
public partial class FilterRoot<T>
    where T : new()
{
    /// <summary>
    /// Filter class.
    /// </summary>
    public string Classname => default(CssBuilder).AddClass(Class).Build();

    /// <summary>
    /// Main content to display.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Filter content to display.
    /// </summary>
    [Parameter]
    public RenderFragment? FilterContent { get; set; }

    /// <summary>
    /// Callback used when filtering data.
    /// </summary>
    [Parameter]
    public Func<Task>? OnFilter { get; set; }

    /// <summary>
    /// Callback used to create a default model.
    /// </summary>
    [Parameter]
    public Func<T> ModelDefaultValueFunc { get; set; } = () => new T();

    /// <summary>
    /// Filter model. Must have a parameterless constructor.
    /// </summary>
    [Parameter]
    public T Model { get; set; } = new();

    /// <summary>
    /// A callback indicating the changes of the <see cref="Model">Model</see> parameter.
    /// </summary>
    [Parameter]
    public EventCallback<T> ModelChanged { get; set; }

    private int _filterCount;

    private MudForm? _form;

    /// <summary>
    /// Filter form.
    /// </summary>
    public MudForm? Form
    {
        get => _form;
        set
        {
            _form = value;

            if (value != null)
                _form.FieldChanged = new EventCallback<FormFieldChangedEventArgs>(this, OnFieldChangedAsync);
        }
    }


    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await UpdateHasFilterAsync();
    }

    private async Task OnFieldChangedAsync()
    {
        await Task.Delay(200);
        await UpdateHasFilterAsync();
        FilterAsync();
    }

    /// <summary>
    /// Filter data.
    /// </summary>
    public async Task FilterAsync()
    {
        await UpdateHasFilterAsync();

        if (OnFilter == null)
            return;

        if (!await _form!.ValidateForm())
            return;

        await OnFilter();
    }

    private async Task UpdateHasFilterAsync()
    {
        _filterCount = await CreateFilterCount();
    }

    /// <summary>
    /// Counts the model filtered properties.
    /// </summary>
    /// <returns>The number of filtered properties.</returns>
    protected virtual async Task<int> CreateFilterCount()
    {
        var result = 0;

        var defaultModel = await CreateDefaultModelAsync();

        foreach (var property in typeof(T).GetProperties())
        {
            var propertyValue = property.GetValue(Model);
            var defaultValue = property.GetValue(defaultModel);

            if (typeof(IList).IsAssignableFrom(property.PropertyType) && propertyValue != null && defaultValue != null)
            {
                var propertyValueAsList = (IList)propertyValue;
                var defaultValueAsList = (IList)propertyValue;

                if (propertyValueAsList.Count != defaultValueAsList.Count)
                    continue;

                var equalItemsCount = 0;

                for (var i = 0; i < propertyValueAsList.Count; i++)
                {
                    if (!CompareValues(propertyValueAsList[i], defaultValueAsList[i]))
                        break;

                    equalItemsCount++;
                }

                if (equalItemsCount == propertyValueAsList.Count)
                    result++;
            }
            else if (CompareValues(propertyValue, defaultValue))
            {
                result++;
            }
        }

        return result;
    }

    private bool CompareValues(object? value, object? defaultValue) => defaultValue != null && !defaultValue!.Equals(value) || defaultValue == null && value != null;

    /// <summary>
    /// Creates a model with default values.
    /// </summary>
    /// <returns>Default model.</returns>
    protected virtual Task<T> CreateDefaultModelAsync() => Task.FromResult(ModelDefaultValueFunc());

    /// <summary>
    /// Resets the filter to its default state.
    /// </summary>
    public async Task ResetAsync()
    {
        Model = await CreateDefaultModelAsync();
        await UpdateHasFilterAsync();
        await ModelChanged.InvokeAsync(Model);
        FilterAsync();
    }
}
