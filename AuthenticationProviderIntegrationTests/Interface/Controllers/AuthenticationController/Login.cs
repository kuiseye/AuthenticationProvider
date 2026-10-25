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

public class LoginTests(ControllerWebFactory webFactory) : ControllerTestsBase(webFactory)
{
    private ControllerWebFactory _webFactory = webFactory;

    [Fact]
    public async Task ShouldReturnSuccessAndSetAppropriateCookie_WhenCredentialsAreCorrect()
    {
        //Arange
        var loginForm = new LoginAppUserForm("username", "password");
        var accessToken = new AccessTokenDto(loginForm.Username);

        _webFactory.AuthenticationService
        .LoginAsync(loginForm, Arg.Any<CancellationToken>())
        .Returns(Result.Success(accessToken));
        //Act
        var response = await _client.PostAsJsonAsync("login?includeToken=true", loginForm);
        //Assert

        //Code
        Assert.InRange((int)response.StatusCode, 200, 299);
        //Body
        var responseBody = await response.Content.ReadFromJsonAsync<AccessTokenDto>();
        Assert.Equal(accessToken, responseBody);

        //Cookies
        var tokenOptions = _scope.ServiceProvider.GetRequiredService<IOptions<TokenSettings>>();
        var cookieOptions = _scope.ServiceProvider.GetRequiredService<IOptions<CookieSettings>>();

        var cookieName = tokenOptions.Value.CookieName; 

        //Parse
        Assert.True(response.Headers.TryGetValues("Set-Cookie", out var raw));
        var cookies = SetCookieHeaderValue.ParseList(raw.ToList());

        var authCookie = cookies.SingleOrDefault(cookie=>cookie.Name == cookieName);

        //Test
        Assert.NotNull(authCookie);

        Assert.NotNull(authCookie.MaxAge);
        Assert.True(tokenOptions.Value.Lifetime.Subtract(authCookie.MaxAge.Value).Duration() < TimeSpan.FromSeconds(1));

        Assert.Equal(cookieOptions.Value.HttpOnly, authCookie.HttpOnly);
        Assert.Equal(cookieOptions.Value.Secure, authCookie.Secure);
        Assert.Equal(cookieOptions.Value.SameSiteMode.ToString(), authCookie.SameSite.ToString());
    }

    [Fact]
    public async Task ShouldReturnOk_WhenIncludeTokenQueryParamIsTrue()
    {
        //Arrange
        var loginForm = new LoginAppUserForm("username", "password");
        var token = new AccessTokenDto(loginForm.Username);
        _webFactory.AuthenticationService
        .LoginAsync(loginForm, Arg.Any<CancellationToken>())
        .Returns(Result.Success(token));
        //Act
        var response = await _client.PostAsJsonAsync("login?includeToken=true",loginForm);
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(token, await response.Content.ReadFromJsonAsync<AccessTokenDto>());
    }

    [Fact]
    public async Task ShouldReturnNoContent_WhenIncludeTokenQueryParamIsAbsent()
    {
        //Arrange
        var loginForm = new LoginAppUserForm("username", "password");
        var token = new AccessTokenDto(loginForm.Username);
        _webFactory.AuthenticationService
        .LoginAsync(loginForm, Arg.Any<CancellationToken>())
        .Returns(Result.Success(token));
        //Act
        var response = await _client.PostAsJsonAsync("login",loginForm);
        //Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnNoContent_WhenIncludeTokenQueryParamIsFalse()
    {
        //Arrange
        var loginForm = new LoginAppUserForm("username", "password");
        var token = new AccessTokenDto(loginForm.Username);
        _webFactory.AuthenticationService
        .LoginAsync(loginForm, Arg.Any<CancellationToken>())
        .Returns(Result.Success(token));
        //Act
        var response = await _client.PostAsJsonAsync("login?includeToken=false",loginForm);
        //Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}