namespace AutSoft.AspNetCore.Blazor.Loading;

/// <summary>
/// Event args for change of loading state.
/// </summary>
public class LoadingStateChangedEventArgs : EventArgs
{
    /// <summary>
    /// Constructor of the LoadingStateChangedEventArgs.
    /// </summary>
    /// <param name="oldState">Old state.</param>
    /// <param name="newState">New state.</param>
    public LoadingStateChangedEventArgs(LoadingStateType oldState, LoadingStateType newState)
    {
        OldState = oldState;
        NewState = newState;
    }

    /// <summary>
    /// New state.
    /// </summary>
    public LoadingStateType NewState { get; }

    /// <summary>
    /// Old state.
    /// </summary>
    public LoadingStateType OldState { get; }
}
