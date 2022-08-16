namespace AutSoft.Common.Exceptions;

/// <summary>
/// Exception for business related validation errors
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1194:Implement exception constructors.", Justification = "Egyedileg nem kell támogatni")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "This exception will not be serialized.")]
public class ValidationException : BusinessException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class.
    /// </summary>
    /// <param name="errors">Errors as key-value pairs</param>
    /// <param name="message">Exception's message</param>
    /// <param name="title">Exception's title for ProblemDetails</param>
    /// <param name="type">Error's type identifier for ProblemDetails</param>
    /// <param name="innerException">Related tiggering exception</param>
    public ValidationException(
        Dictionary<string, string> errors,
        string? message = null,
        string? title = null,
        Uri? type = null,
        Exception? innerException = null)
        : base(message, innerException, title, type)
    {
        Errors = errors;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class.
    /// </summary>
    /// <param name="errorKey">Single error's key</param>
    /// <param name="errorValue">Single error's message</param>
    /// <param name="message">Exception's message</param>
    /// <param name="title">Exception's title for ProblemDetails</param>
    /// <param name="type">Error's type identifier for ProblemDetails</param>
    /// <param name="innerException">Related tiggering exception</param>
    public ValidationException(
        string errorKey,
        string errorValue,
        string? message = null,
        string? title = null,
        Uri? type = null,
        Exception? innerException = null)
        : this(new Dictionary<string, string> { { errorKey, errorValue } }, message, title, type, innerException)
    {
    }

    /// <summary>
    /// Gets validation errors as key-value pairs
    /// </summary>
    public Dictionary<string, string> Errors { get; }
}
