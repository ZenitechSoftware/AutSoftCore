using AutSoft.Common.Concurrency;
using AutSoft.Linq.Models;
using AutSoft.Mud.Blazor.Loading;

using Microsoft.AspNetCore.Components;

namespace AutSoft.Mud.Blazor.InfiniteScroll;

/// <summary>
/// Infinite scroll.
/// </summary>
/// <typeparam name="TItem">Type of the list items.</typeparam>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "<Pending>")]
public partial class InfiniteScroll<TItem>
{
    private readonly AsyncLock _loadMoreLock = new();
    private readonly string _observerTargetId = Guid.NewGuid().ToString();
    private int _totalServiceRequestCount;
    private readonly object _reloadLock = new();
    private bool _isReloading;

    /// <summary>
    /// Indicates auto load status.
    /// </summary>
    [Parameter]
    public bool Autoload { get; set; } = false;

    /// <summary>
    /// Item template.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem> ItemTemplate { get; set; } = default!;

    /// <summary>
    /// No items content.
    /// </summary>
    [Parameter]
    public RenderFragment NoItemsContent { get; set; } = default!;

    /// <summary>
    /// Page size.
    /// </summary>
    [Parameter]
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Items.
    /// </summary>
    [Parameter]
    public List<TItem> Items { get; set; } = new List<TItem>();

    /// <summary>
    /// Event callback indicates when the items have changed.
    /// </summary>
    [Parameter]
    public EventCallback<List<TItem>> ItemsChanged { get; set; }

    /// <summary>
    /// Callback used to load more items.
    /// </summary>
    [Parameter]
    public Func<int, int, Task<PageResponse<TItem>>> LoadMoreItems { get; set; } = default!;

    /// <summary>
    /// Indicates if the list has more items.
    /// </summary>
    public bool HasMoreItems => _totalServiceRequestCount > Items.Count;

    [Inject]
    private LoadingOperation RootLoading { get; set; } = default!;

    /// <summary>
    /// Constructor of the InfiniteScroll.
    /// </summary>
    public InfiniteScroll()
    {
        NoItemsContent = (builder) =>
        {
            builder.OpenComponent<NoItemsContent>(0);
            builder.CloseComponent();
        };
    }

    /// <inheritdoc />
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        RootLoading.IsBlocking = true;
        await base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (Items.Count > 0)
            RootLoading.Done();

        if (Autoload)
            await ReloadAsync();
    }

    /// <summary>
    /// Reloads the list.
    /// </summary>
    public async Task ReloadAsync()
    {
        lock (_reloadLock)
        {
            if (_isReloading)
                return;

            _isReloading = true;
        }

        await RootLoading.RunAsync(async () =>
        {
            Items = new List<TItem>();
            await ItemsChanged.InvokeAsync(Items);

            var response = await LoadMoreItems(0, PageSize);
            _totalServiceRequestCount = response.TotalCount;

            Items = response.Results;
            await ItemsChanged.InvokeAsync(Items);
        });

        lock (_reloadLock)
        {
            _isReloading = false;
        }
    }

    private async Task LoadMoreItemsAsync()
    {
        if (!HasMoreItems)
            return;

        lock (_reloadLock)
        {
            if (_isReloading)
                return;
        }

        using (await AsyncLockContext.CreateAsync(_loadMoreLock))
        {
            var response = await LoadMoreItems(Items.Count / PageSize, PageSize);
            _totalServiceRequestCount = response.TotalCount;
            Items.AddRange(response.Results);
        }
    }
}
