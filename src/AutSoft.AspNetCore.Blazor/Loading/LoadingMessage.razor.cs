using Microsoft.AspNetCore.Components;

namespace AutSoft.AspNetCore.Blazor.Loading;

/// <summary>
/// Loading message display.
/// </summary>
public partial class LoadingMessage
{
    /// <summary>
    /// Message to display.
    /// </summary>
    [Parameter]
    public string Message { get; set; } = string.Empty;
}
