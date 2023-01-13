using Microsoft.AspNetCore.Components;

using MudBlazor;

namespace AutSoft.Mud.Blazor.Dialog;

/// <summary>
/// Base class for dialogs.
/// </summary>
public abstract class DialogComponentBase : ComponentBase
{
    /// <summary>
    /// Current dialog instance.
    /// </summary>
    [CascadingParameter]
    public MudDialogInstance? DialogReference { get; set; }

    /// <summary>
    /// Cancel dialog.
    /// </summary>
    protected void Cancel()
    {
        DialogReference?.Cancel();
    }
}
