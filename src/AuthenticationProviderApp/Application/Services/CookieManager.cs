using AuthenticationProvider.Configuration.Settings;
using Microsoft.Extensions.Options;

namespace AuthenticationProvider.Application.Services;

public class CookieManager : ICookieManager
{
    private IOptions<CookieSettings> _cookieOptions;
    private IOptions<TokenSettings> _tokenOptions;
    private HttpContext _httpContext;
    public CookieManager(IOptions<CookieSettings> cookieOptions, IOptions<TokenSettings> tokenOptions, IHttpContextAccessor httpContextAccessor)
    {
        _cookieOptions = cookieOptions;
        _tokenOptions = tokenOptions;
        _httpContext = httpContextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor), "Cookie manager could not access HttpContext from an accessor, the value is null");
    }
    public void AppendTokenToCookies(string token)
    {
        var cookieSettings = _cookieOptions.Value;
        _httpContext.Response.Cookies.Append(_tokenOptions.Value.CookieName, token, new CookieOptions
        {
            HttpOnly = cookieSettings.HttpOnly,
            Secure = cookieSettings.Secure,
            IsEssential = true,
            SameSite = cookieSettings.SameSiteMode,
            MaxAge = _tokenOptions.Value.Lifetime
        });
    }
}