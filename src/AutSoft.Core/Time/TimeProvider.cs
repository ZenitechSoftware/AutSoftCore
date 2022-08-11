namespace AutSoft.Common.Time;

public class TimeProvider : ITimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

    public DateTimeOffset Now => DateTimeOffset.Now;
}
