namespace AutSoft.AspNetCore.Blazor.Dialog.Error;

/// <summary>
/// Error dialog.
/// </summary>
public partial class ErrorDialog
{
    /// <summary>
    /// Copy error details to the clipboard.
    /// </summary>
    public void OnCopyDetails()
    {
        if (Parameter!.OnCopyDetails == null)
            return;

        Parameter.OnCopyDetails(Parameter.Error);
    }
}
