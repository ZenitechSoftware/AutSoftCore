using AutSoft.AspNetCore.Blazor.ErrorHandling;

namespace AutSoft.Mud.Blazor.Dialog.Error;

/// <summary>
/// Parameter of error dialog.
/// </summary>
public class ErrorDialogParameter
{
    /// <summary>
    /// Constructor of the ErrorDialogParameter.
    /// </summary>
    /// <param name="error">Display error.</param>
    /// <param name="onCopyDetails">Callback used when copying details.</param>
    public ErrorDialogParameter(DisplayError error, Func<DisplayError, Task>? onCopyDetails = null)
    {
        Error = error;
        OnCopyDetails = onCopyDetails;
    }

    /// <summary>
    /// Display error.
    /// </summary>
    public DisplayError Error { get; }

    /// <summary>
    /// Callback used when copying details.
    /// </summary>
    public Func<DisplayError, Task>? OnCopyDetails { get; }
}
