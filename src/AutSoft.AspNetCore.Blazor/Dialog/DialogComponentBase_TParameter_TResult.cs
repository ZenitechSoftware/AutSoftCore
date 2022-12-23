namespace AutSoft.AspNetCore.Blazor.Dialog;

/// <inheritdoc />
public abstract class DialogComponentBase<TParameter, TResult> : DialogComponentBase<TParameter>
{
    /// <summary>
    /// Close dialog.
    /// </summary>
    protected void Close(TResult result)
    {
        DialogReference?.Close(result);
    }
}
