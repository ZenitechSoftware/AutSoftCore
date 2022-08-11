namespace AutSoft.Common.Time;

/// <summary>
/// Time abstraction on top of <see cref="DateTimeOffset"/> for mocking purposes
/// </summary>
public interface ITimeProvider
{
    /// <summary>
    /// Gets current UTC time
    /// </summary>
    public DateTimeOffset UtcNow { get; }

    /// <summary>
    /// Gets current local time
    /// </summary>
    public DateTimeOffset Now { get; }
}
