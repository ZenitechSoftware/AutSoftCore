namespace AutSoft.AspNetCore.Blazor.Autocomplete;

/// <summary>
/// Autocomplete field data source.
/// </summary>
/// <typeparam name="TKey">Key type.</typeparam>
/// <typeparam name="TItem">Item type.</typeparam>
public class AutoCompleteDataSource<TKey, TItem>
{
    private readonly Func<string, Task<IEnumerable<TItem>>> _search;
    private readonly Func<TItem, TKey> _keySelector;
    private readonly Func<TItem, string> _nameSelector;
    private IEnumerable<TItem>? _currentItems;

    /// <summary>
    /// Constructor of the AutoCompleteDataSource.
    /// </summary>
    /// <param name="search">Search function.</param>
    /// <param name="keySelector">Key selector.</param>
    /// <param name="nameSelector">Name selector.</param>
    public AutoCompleteDataSource(Func<string, Task<IEnumerable<TItem>>> search, Func<TItem, TKey> keySelector, Func<TItem, string> nameSelector)
    {
        _search = search;
        _keySelector = keySelector;
        _nameSelector = nameSelector;
    }

    /// <summary>
    /// Gets the keys of the searched items.
    /// </summary>
    /// <param name="search">Search term.</param>
    /// <returns>Items keys.</returns>
    public async Task<IEnumerable<TKey>> SearchAsync(string search)
    {
        _currentItems = await _search(search);
        return _currentItems.Select(i => _keySelector(i));
    }

    /// <summary>
    /// Sets the currently selected item.
    /// </summary>
    /// <param name="item">Selected item.</param>
    public void SetCurrentItem(TItem? item)
    {
        if (EqualityComparer<TItem>.Default.Equals(item, default))
        {
            _currentItems = new List<TItem>();
            return;
        }

        _currentItems = new List<TItem> { item! };
    }

    private TItem? GetSelectedItem(TKey? key)
    {
        if (_currentItems == null)
            return default;

        return _currentItems.SingleOrDefault(i => EqualityComparer<TKey>.Default.Equals(key, _keySelector(i)));
    }

    /// <summary>
    /// Gets the name of the item with the specified key.
    /// </summary>
    /// <param name="key">Key of the item.</param>
    /// <returns>Name of the item.</returns>
    public string? GetItemName(TKey? key)
    {
        if (EqualityComparer<TKey>.Default.Equals(key, default))
            return null;

        var selectedItem = GetSelectedItem(key);

        return !EqualityComparer<TItem>.Default.Equals(selectedItem, default) ? _nameSelector(selectedItem!) : null;
    }
}
