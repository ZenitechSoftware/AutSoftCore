namespace AutSoft.Common.Exceptions;

/// <summary>
/// Exception which represents business errors
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1194:Implement exception constructors.", Justification = "Custom constructors are provided")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "This exception will not be serialized.")]
public class BusinessException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessException"/> class.
    /// </summary>
    /// <param name="message">Exception's message</param>
    /// <param name="title">Exception's title for ProblemDetails</param>
    /// <param name="type">Error's type identifier for ProblemDetails</param>
    public BusinessException(
        string? message,
        string? title = null,
        Uri? type = null)
        : this(message, null, title, type)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessException"/> class.
    /// </summary>
    /// <param name="message">Exception's message</param>
    /// <param name="innerException">Related tiggering exception</param>
    /// <param name="title">Exception's title for ProblemDetails</param>
    /// <param name="type">Error's type identifier for ProblemDetails</param>
    public BusinessException(
        string? message,
        Exception? innerException,
        string? title = null,
        Uri? type = null)
        : base(message, innerException)
    {
        Title = title;
        Type = type;
    }

    /// <summary>
    /// Gets exception's title (mostly for problem details)
    /// </summary>
    public string? Title { get; }

    /// <summary>
    /// Gets error's type identifier (mostly for problem details)
    /// </summary>
    public Uri? Type { get; }
}
