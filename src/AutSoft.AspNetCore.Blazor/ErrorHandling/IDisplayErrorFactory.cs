namespace AutSoft.AspNetCore.Blazor.ErrorHandling;

/// <summary>
/// Factory for <see cref="DisplayError">DisplayError</see>.
/// </summary>
public interface IDisplayErrorFactory
{
    /// <summary>
    /// Create <see cref="DisplayError">DisplayError</see> from <see cref="Exception">Exception</see>.
    /// </summary>
    DisplayError CreateDisplayError(Exception exception);
}
