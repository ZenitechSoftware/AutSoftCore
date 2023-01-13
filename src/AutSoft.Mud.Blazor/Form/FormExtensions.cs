using MudBlazor;

namespace AutSoft.Mud.Blazor.Form;

/// <summary>
/// Extension methods for <see cref="MudForm">MudForm</see>.
/// </summary>
public static class FormExtensions
{
    /// <summary>
    /// Validates the form and returns with the validation result.
    /// </summary>
    public static async Task<bool> ValidateForm(this MudForm? form)
    {
        if (form == null)
            return false;

        await form.Validate();
        return form.IsValid;
    }
}
