namespace AutSoft.Common.Exceptions;

/// <summary>
/// Exception when user don't have enough permission to perform the action
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1194:Implement exception constructors.", Justification = "Custom constructors are provided")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "This exception will not be serialized.")]
public class ForbiddenException : BusinessException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
    /// </summary>
    /// <param name="message">Exception's message</param>
    /// <param name="title">Exception's title for ProblemDetails</param>
    /// <param name="type">Error's type identifier for ProblemDetails</param>
    public ForbiddenException(string? message = null, string? title = null, Uri? type = null)
        : base(message, title, type)
    {
    }
}
