namespace AutSoft.Common.Concurrency;

/// <summary>
/// Async lock.
/// </summary>
public class AsyncLock : SemaphoreSlim
{
    /// <summary>
    /// Constructor of the AsyncLock.
    /// </summary>
    public AsyncLock()
        : base(1)
    {
    }
}
