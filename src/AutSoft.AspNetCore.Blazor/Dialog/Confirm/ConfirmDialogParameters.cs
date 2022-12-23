namespace AutSoft.AspNetCore.Blazor.Dialog.Confirm;


/// <summary>
/// Parameters for the confirmation dialog.
/// </summary>
/// <param name="Title">Title of the dialog.</param>
/// <param name="Description">Description of the dialog.</param>
public record ConfirmDialogParameters(string Title, string Description);
