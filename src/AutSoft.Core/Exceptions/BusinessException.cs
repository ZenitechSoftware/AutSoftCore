namespace AutSoft.Common.Exceptions;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1194:Implement exception constructors.", Justification = "Custom constructors are provided")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "This exception will not be serialized.")]
public class BusinessException : Exception
{
    public BusinessException(
        string? message,
        string? title = null,
        Uri? type = null)
        : this(message, null, title, type)
    {
    }

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

    public string? Title { get; }

    public Uri? Type { get; }
}
