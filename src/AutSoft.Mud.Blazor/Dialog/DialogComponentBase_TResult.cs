namespace AutSoft.Mud.Blazor.Dialog;

/// <summary>
/// Base class for dialog components with return value.
/// </summary>
/// <typeparam name="TResult">Return value type.</typeparam>
public class DialogComponentBaseWithResult<TResult> : DialogComponentBase
{
    /// <summary>
    /// Close dialog.
    /// </summary>
    protected void Close(TResult result)
    {
        DialogReference?.Close(result);
    }
}
