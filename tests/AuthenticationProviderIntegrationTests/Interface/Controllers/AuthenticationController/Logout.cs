using System.Net;
using System.Net.Http.Json;
using AuthenticationProvider.Application;
using AuthenticationProvider.Application.Dtos.AppUserDtos;
using AuthenticationProvider.Application.Dtos.TokenDtos;
using AuthenticationProvider.Configuration.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using NSubstitute;

namespace AuthenticationProvider.Tests.Integration.Interface.Controllers.AuthenticationController;

public class LogoutTests(ControllerWebFactory webFactory) : ControllerTestsBase(webFactory)
{
    private ControllerWebFactory _webFactory = webFactory;

    [Fact]
    public async Task ShouldReturnSuccessAndDeleteToeknCookie_WhenLogedIn()
    {
        //Arange
        var loginForm = new LoginAppUserForm("username", "password");
        var accessToken = new AccessTokenDto(loginForm.Username);

        _webFactory.AuthenticationService
        .LoginAsync(loginForm, Arg.Any<CancellationToken>())
        .Returns(Result.Success(accessToken));
        var postResponse = await _client.PostAsJsonAsync("login", loginForm);
        //Act
        var response = await _client.PostAsync("logout", null);
        //Assert
        Assert.InRange((int)postResponse.StatusCode, 200, 299);
        Assert.InRange((int)response.StatusCode, 200, 299);

        //Cookies
        var tokenOptions = _scope.ServiceProvider.GetRequiredService<IOptions<TokenSettings>>();

        var cookieName = tokenOptions.Value.CookieName; 

        //Parse
        Assert.True(response.Headers.TryGetValues("Set-Cookie", out var raw));
        var cookies = SetCookieHeaderValue.ParseList(raw.ToList());

        var authCookie = cookies.SingleOrDefault(cookie=>cookie.Name == cookieName);
        //Test
        Assert.NotNull(authCookie);

        Assert.NotNull(authCookie.Expires);
        Assert.True(authCookie.Expires.Value < DateTimeOffset.UtcNow);
    }

    [Fact]
    public async Task ShouldReturnSuccessAndDeleteToeknCookie_Always()
    {
        //Act
        var response = await _client.PostAsync("logout", null);
        //Assert
        Assert.InRange((int)response.StatusCode, 200, 299);

        //Cookies
        var tokenOptions = _scope.ServiceProvider.GetRequiredService<IOptions<TokenSettings>>();

        var cookieName = tokenOptions.Value.CookieName; 

        //Parse
        Assert.True(response.Headers.TryGetValues("Set-Cookie", out var raw));
        var cookies = SetCookieHeaderValue.ParseList(raw.ToList());

        var authCookie = cookies.SingleOrDefault(cookie=>cookie.Name == cookieName);
        //Test
        Assert.NotNull(authCookie);

        Assert.NotNull(authCookie.Expires);
        Assert.True(authCookie.Expires.Value < DateTimeOffset.UtcNow);
    }
}