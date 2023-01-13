namespace AutSoft.Mud.Blazor.Loading;

/// <summary>
/// Error handler interface.
/// </summary>
public interface ILoadingErrorHandler
{
    /// <summary>
    /// Handle error.
    /// </summary>
    Task HandleErrorAsync(LoadingOperation loadingOperation, Exception exception);
}
