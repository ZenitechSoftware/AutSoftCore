namespace AutSoft.Common.Concurrency;

/// <summary>
/// Context for async lock.
/// </summary>
public class AsyncLockContext : IDisposable
{
    private SemaphoreSlim? _asyncLock;

    /// <summary>
    /// Constructor of the AsyncLockContext.
    /// </summary>
    /// <param name="asyncLock">Async lock.</param>
    protected AsyncLockContext(SemaphoreSlim asyncLock)
    {
        _asyncLock = asyncLock;
    }

    /// <summary>
    /// Creates context.
    /// </summary>
    public static async Task<AsyncLockContext> CreateAsync(SemaphoreSlim asyncLock)
    {
        await asyncLock.WaitAsync();
        return new AsyncLockContext(asyncLock);
    }

    /// <summary>
    /// Creates context with timeout.
    /// </summary>
    public static async Task<AsyncLockContext> CreateAsync(SemaphoreSlim asyncLock, int millisecondsTimeout)
    {
        await asyncLock.WaitAsync(millisecondsTimeout);
        return new AsyncLockContext(asyncLock);
    }

    private bool _disposed;

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _asyncLock?.Release();
            _asyncLock = null;
        }

        _disposed = true;
    }
}
