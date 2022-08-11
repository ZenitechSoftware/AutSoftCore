namespace AutSoft.Common.Exceptions;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1194:Implement exception constructors.", Justification = "Egyedileg nem kell támogatni")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "This exception will not be serialized.")]
public class ValidationException : BusinessException
{
    public ValidationException(
        List<ValidationError> errors,
        string? message = null,
        string? title = null,
        Uri? type = null,
        Exception? innerException = null)
        : base(message, innerException, title, type)
    {
        Errors = errors;
    }

    public ValidationException(
        ValidationError error,
        string? message = null,
        string? title = null,
        Uri? type = null,
        Exception? innerException = null)
        : this(new List<ValidationError> { error }, message, title, type, innerException)
    {
    }

    public ValidationException(
        string key,
        string error,
        string? message = null,
        string? title = null,
        Uri? type = null,
        Exception? innerException = null)
        : this(new ValidationError(key, error), message, title, type, innerException)
    {
    }

    public List<ValidationError> Errors { get; }
}
