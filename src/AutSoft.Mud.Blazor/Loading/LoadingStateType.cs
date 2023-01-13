namespace AutSoft.Mud.Blazor.Loading;

/// <summary>
/// Loading state.
/// </summary>
public enum LoadingStateType
{
    /// <summary>
    /// Currently loading.
    /// </summary>
    Loading = 1,

    /// <summary>
    /// Loading is done, operation was successful.
    /// </summary>
    Done = 2,

    /// <summary>
    /// Loading is done, operation failed.
    /// </summary>
    Failed = 3,
}
