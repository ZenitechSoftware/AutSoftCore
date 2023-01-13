namespace AutSoft.Mud.Blazor.Loading;

/// <summary>
/// Event args for change of blocking state.
/// </summary>
public class BlockingStateChangedEventArgs : EventArgs
{
    /// <summary>
    /// Constructor of the BlockingStateChangedEventArgs.
    /// </summary>
    /// <param name="oldState">Old state.</param>
    /// <param name="newState">New state.</param>
    public BlockingStateChangedEventArgs(bool oldState, bool newState)
    {
        OldState = oldState;
        NewState = newState;
    }

    /// <summary>
    /// New state.
    /// </summary>
    public bool NewState { get; }

    /// <summary>
    /// Old state.
    /// </summary>
    public bool OldState { get; }
}
