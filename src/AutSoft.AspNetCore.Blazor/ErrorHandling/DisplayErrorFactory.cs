using AutSoft.Common.Exceptions;

namespace AutSoft.AspNetCore.Blazor.ErrorHandling;

/// <inheritdoc />
public class DisplayErrorFactory : IDisplayErrorFactory
{
    /// <inheritdoc />
    public DisplayError CreateDisplayError(Exception exception)
    {
        return new DisplayError(
            title: "Error",
            details: exception.Message,
            technicalDetails: exception.ToString());
    }
}
