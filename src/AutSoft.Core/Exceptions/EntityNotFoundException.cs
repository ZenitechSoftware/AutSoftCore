namespace AutSoft.Common.Exceptions;

/// <summary>
/// Exception which occuers when request queries a not existed entity.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1194:Implement exception constructors.", Justification = "Prefer factory methods")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "This exception will not be serialized.")]
public sealed class EntityNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
    /// </summary>
    /// <param name="message">Exception's message</param>
    /// <param name="innerException">Related tiggering exception</param>
    private EntityNotFoundException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Foctory method which creates an <see cref="EntityNotFoundException"/> with a standard message.
    /// </summary>
    /// <typeparam name="T">Not found entity's type</typeparam>
    /// <param name="id">Entity's id</param>
    /// <returns>Returns <see cref="EntityNotFoundException"/> with a standard message.</returns>
    public static EntityNotFoundException CreateForType<T>(long? id)
        => new($"Cannot find entity of type {typeof(T).Name}" + (id.HasValue ? $" with id={id}" : string.Empty));

    /// <summary>
    /// Foctory method which creates an <see cref="EntityNotFoundException"/> with a standard message.
    /// </summary>
    /// <typeparam name="T">Not found entity's type</typeparam>
    /// <param name="innerException">Related tiggering exception</param>
    /// <param name="id">Entity's id</param>
    /// <returns>Returns <see cref="EntityNotFoundException"/> with a standard message.</returns>
    public static EntityNotFoundException CreateForType<T>(Exception innerException, long? id)
        => new($"Cannot find entity of type {typeof(T).Name}" + (id.HasValue ? $" with id={id}" : string.Empty), innerException);

    /// <summary>
    /// Foctory method which creates an <see cref="EntityNotFoundException"/> with a standard message.
    /// </summary>
    /// <typeparam name="T">Not found entity's type</typeparam>
    /// <param name="queryParameters">Search parameters which occur this error</param>
    /// <returns>Returns <see cref="EntityNotFoundException"/> with a standard message.</returns>
    public static EntityNotFoundException CreateForTypeCustomParams<T>(params object[] queryParameters)
        => new($"Cannot find entity of type {typeof(T).Name} with params:" + string.Join("; ", queryParameters));

    /// <summary>
    /// Foctory method which creates an <see cref="EntityNotFoundException"/> with a standard message.
    /// </summary>
    /// <typeparam name="T">Not found entity's type</typeparam>
    /// <param name="innerException">Related tiggering exception</param>
    /// <param name="queryParameters">Search parameters which occur this error</param>
    /// <returns>Returns <see cref="EntityNotFoundException"/> with a standard message.</returns>
    public static EntityNotFoundException CreateForTypeCustomParams<T>(Exception innerException, params object[] queryParameters)
        => new($"Cannot find entity of type {typeof(T).Name} with params:" + string.Join("; ", queryParameters), innerException);
}
