namespace AutSoft.AspNetCore.Blazor.Loading;

/// <summary>
/// Loading state.
/// </summary>
public enum LoadingStateType
{
    /// <summary>
    /// Currently loading.
    /// </summary>
    Loading = 0,

    /// <summary>
    /// Loading is done, operation was successful.
    /// </summary>
    Done = 1,

    /// <summary>
    /// Loading is done, operation failed.
    /// </summary>
    Failed = 2,
}
