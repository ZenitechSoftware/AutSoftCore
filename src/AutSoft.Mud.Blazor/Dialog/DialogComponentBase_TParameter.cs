using Microsoft.AspNetCore.Components;

namespace AutSoft.Mud.Blazor.Dialog;

/// <summary>
/// Base class for dialog components with parameter.
/// </summary>
/// <typeparam name="TParameter">Parameter type.</typeparam>
public abstract class DialogComponentBase<TParameter> : DialogComponentBase
{
    /// <summary>
    /// Parameter of the dialog.
    /// </summary>
    [Parameter]
    public TParameter? Parameter { get; set; }
}
