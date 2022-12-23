using AutSoft.AspNetCore.Blazor.Clipboard;
using AutSoft.AspNetCore.Blazor.ErrorHandling;

using Microsoft.Extensions.Logging;

using MudBlazor;

namespace AutSoft.AspNetCore.Blazor.Loading;

/// <summary>
/// Loading operation.
/// </summary>
public class LoadingOperation
{
    private readonly ILogger _logger;
    private readonly IDisplayErrorFactory _displayErrorFactory;
    private readonly DefaultLoadingErrorHandlerFactory _defaultLoadingErrorHandlerFactory;
    private readonly IClipboardService _clipboardService;
    private readonly ISnackbar _snackbar;

    private Func<Task> _currentBody = () => Task.CompletedTask;
    private ILoadingErrorHandler? _currentErrorHandler;

    /// <summary>
    /// Loading operation state.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "Base service")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Base service")]
    protected LoadingStateType _state = LoadingStateType.Done;

    /// <summary>
    /// Loading operation blocking state.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "Base service")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Base service")]
    protected bool _isBlocking;

    /// <summary>
    /// Constructor of the LoadingOperation.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="displayErrorFactory">Display error factory.</param>
    /// <param name="defaultLoadingErrorHandlerFactory">Default loading error handler factory.</param>
    /// <param name="clipboardService">Clipboard service.</param>
    /// <param name="snackbar"></param>
    public LoadingOperation(
    ILogger<LoadingOperation> logger,
    IDisplayErrorFactory displayErrorFactory,
    DefaultLoadingErrorHandlerFactory defaultLoadingErrorHandlerFactory,
    IClipboardService clipboardService,
    ISnackbar snackbar
)
    {
        _logger = logger;
        _displayErrorFactory = displayErrorFactory;
        _defaultLoadingErrorHandlerFactory = defaultLoadingErrorHandlerFactory;
        _clipboardService = clipboardService;
        _snackbar = snackbar;
    }

    /// <summary>
    /// Event indicating the change of state.
    /// </summary>
    public event EventHandler<LoadingStateChangedEventArgs>? StateChanged;

    /// <summary>
    /// Event indicating the change of blocking state.
    /// </summary>
    public event EventHandler<BlockingStateChangedEventArgs>? BlockingChanged;

    /// <summary>
    /// Current state.
    /// </summary>
    public LoadingStateType State
    {
        get => _state;
        set
        {
            var oldState = _state;

            _state = value;
            if (oldState != _state)
                StateChanged?.Invoke(this, new LoadingStateChangedEventArgs(oldState, _state));
        }
    }

    /// <summary>
    /// Is blocking.
    /// </summary>
    public bool IsBlocking
    {
        get => _isBlocking;
        set
        {
            var oldState = _isBlocking;

            _isBlocking = value;
            if (oldState != _isBlocking)
                BlockingChanged?.Invoke(this, new BlockingStateChangedEventArgs(oldState, _isBlocking));
        }
    }

    /// <summary>
    /// Is currently loading.
    /// </summary>
    public bool IsLoading => State == LoadingStateType.Loading;

    /// <summary>
    /// Is currently failed.
    /// </summary>
    public bool IsFailed => State == LoadingStateType.Failed;

    /// <summary>
    /// Is currently done.
    /// </summary>
    public bool IsDone => State == LoadingStateType.Done;

    /// <summary>
    /// Is currently content visible.
    /// </summary>
    public bool IsContentVisible => IsDone || IsLoading && !IsBlocking;

    /// <summary>
    /// Current error.
    /// </summary>
    public DisplayError? Error { get; set; }

    /// <summary>
    /// Encapsulate and run a task in this loading operation. Occuring errors are handled during the execution.
    /// </summary>
    public async Task RunAsync(Func<Task> body, ILoadingErrorHandler? errorHandler = null, bool isRetry = false)
    {
        Loading();

        if (!isRetry)
        {
            _currentBody = body;
            _currentErrorHandler = errorHandler ?? _defaultLoadingErrorHandlerFactory.Create();
        }

        try
        {
            await _currentBody();
            Done();
        }
        catch (Exception e)
        {
            _logger.LogError(e, nameof(LoadingOperation));
            await _currentErrorHandler!.HandleErrorAsync(this, e);
        }
    }

    /// <summary>
    /// Set state to <see cref="LoadingStateType.Loading">Loading</see>.
    /// </summary>
    public virtual void Loading()
    {
        State = LoadingStateType.Loading;
        Error = null;
    }

    /// <summary>
    /// Set state to <see cref="LoadingStateType.Done">Done</see>.
    /// </summary>
    public virtual void Done()
    {
        State = LoadingStateType.Done;
        Error = null;
    }

    /// <summary>
    /// Set state to <see cref="LoadingStateType.Failed">Failed</see> and provide the triggering exception.
    /// </summary>
    public virtual void Failed(Exception exception)
    {
        _logger.LogError(exception, nameof(Failed));

        Error = _displayErrorFactory.CreateDisplayError(exception);
        State = LoadingStateType.Failed;
    }

    /// <summary>
    /// Set state to <see cref="LoadingStateType.Failed">Failed</see> and provide the error to display.
    /// </summary>
    public virtual void Failed(DisplayError error)
    {
        Error = error;
        State = LoadingStateType.Failed;
    }

    /// <summary>
    /// Retry current operation.
    /// </summary>
    public async Task RetryAsync()
    {
        if (IsBlocking)
            await RunAsync(_currentBody, _currentErrorHandler, true);
        else
            Done();
    }

    /// <summary>
    /// Copy details of current error to clipboard.
    /// </summary>
    public Task CopyDetailsAsync() => CopyDetailsAsync(Error!);

    /// <summary>
    /// Copy the error to clipboard.
    /// </summary>
    public async Task CopyDetailsAsync(DisplayError error)
    {
        await _clipboardService.WriteTextAsync(error.ToString());
        _snackbar.Add("Details copied to clipboard");
    }
}
