namespace AuthenticationProvider.Application.Services;

public interface IRequestClock
{
    DateTimeOffset UtcNow {get;}
}