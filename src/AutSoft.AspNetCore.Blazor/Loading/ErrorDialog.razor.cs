namespace AutSoft.AspNetCore.Blazor.Loading;

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
