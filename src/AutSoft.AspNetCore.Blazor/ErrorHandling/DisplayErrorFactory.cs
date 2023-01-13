namespace AutSoft.AspNetCore.Blazor.ErrorHandling;

/// <inheritdoc />
public class DisplayErrorFactory : IDisplayErrorFactory
{
    /// <inheritdoc />
    public virtual DisplayError CreateDisplayError(Exception exception)
    {
        return new DisplayError(
            title: "Error",
            details: exception.Message,
            technicalDetails: exception.ToString());
    }
}
