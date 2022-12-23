using Microsoft.AspNetCore.Components;

namespace AutSoft.AspNetCore.Blazor.ErrorHandling;

public partial class ErrorCard
{
    /// <summary>
    /// Title of the error card.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Message of the error card.
    /// </summary>
    [Parameter]
    public string? Message { get; set; }
}
