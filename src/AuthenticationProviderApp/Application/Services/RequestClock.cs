namespace AuthenticationProvider.Application.Services;

public class RequestClock(TimeProvider timeProvider) : IRequestClock
{
    public DateTimeOffset UtcNow {get;} = timeProvider.GetUtcNow();
}