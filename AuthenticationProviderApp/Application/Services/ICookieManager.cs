namespace AuthenticationProvider.Application.Services;

public interface ICookieManager
{
    void AppendTokenToCookies(string token);
}