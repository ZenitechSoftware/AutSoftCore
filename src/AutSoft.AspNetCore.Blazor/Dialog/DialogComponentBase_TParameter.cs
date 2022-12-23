using Microsoft.AspNetCore.Components;

namespace AutSoft.AspNetCore.Blazor.Dialog;

/// <inheritdoc />
public abstract class DialogComponentBase<TParameter> : DialogComponentBase
{
    /// <summary>
    /// Parameter of the dialog.
    /// </summary>
    [Parameter]
    public TParameter? Parameter { get; set; }
}
