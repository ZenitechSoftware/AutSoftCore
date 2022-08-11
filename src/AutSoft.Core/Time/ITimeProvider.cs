namespace AutSoft.Common.Time;

public interface ITimeProvider
{
    public DateTimeOffset UtcNow { get; }

    public DateTimeOffset Now { get; }
}
