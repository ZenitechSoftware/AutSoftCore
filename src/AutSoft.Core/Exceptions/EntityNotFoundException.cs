namespace AutSoft.Common.Exceptions;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1194:Implement exception constructors.", Justification = "Prefer factory methods")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "This exception will not be serialized.")]
public sealed class EntityNotFoundException : Exception
{
    private EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public static EntityNotFoundException CreateForType<T>(Exception innerException, long? id)
        => new($"Cannot find entity of type {typeof(T).Name}" + (id.HasValue ? $" with id={id}" : string.Empty), innerException);

    public static EntityNotFoundException CreateForTypeCustomParams<T>(Exception innerException, params object[] queryParameters)
        => new($"Cannot find entity of type {typeof(T).Name} with params:" + string.Join("; ", queryParameters), innerException);
}
