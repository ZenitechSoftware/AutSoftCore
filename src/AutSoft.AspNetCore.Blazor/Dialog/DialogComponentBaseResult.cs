namespace AutSoft.AspNetCore.Blazor.Dialog;

/// <inheritdoc />
public class DialogComponentBaseResult<TResult> : DialogComponentBase
{
    /// <summary>
    /// Close dialog.
    /// </summary>
    protected void Close(TResult result)
    {
        DialogReference?.Close(result);
    }
}
