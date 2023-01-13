namespace AutSoft.Mud.Blazor.Dialog;

/// <summary>
/// Base class for dialog components with parameter and return value.
/// </summary>
/// <typeparam name="TParameter">Parameter type.</typeparam>
/// <typeparam name="TResult">Return value type.</typeparam>
public abstract class DialogComponentBaseTParameterTResult<TParameter, TResult> : DialogComponentBaseTParameter<TParameter>
{
    /// <summary>
    /// Close dialog.
    /// </summary>
    protected void Close(TResult result)
    {
        DialogReference?.Close(result);
    }
}
