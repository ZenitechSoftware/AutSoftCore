namespace AutSoft.Common.Exceptions;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1194:Implement exception constructors.", Justification = "Egyedileg nem kell támogatni")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "This exception will not be serialized.")]
public class ForbiddenException : BusinessException
{
    public ForbiddenException(string? message = null, string? title = null, Uri? type = null)
        : base(message, title, type)
    {
    }
}
