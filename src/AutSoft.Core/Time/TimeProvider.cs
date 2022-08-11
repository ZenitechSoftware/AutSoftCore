namespace AutSoft.Common.Time;

/// <inheritdoc />
public class TimeProvider : ITimeProvider
{
    /// <inheritdoc />
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

    /// <inheritdoc />
    public DateTimeOffset Now => DateTimeOffset.Now;
}
