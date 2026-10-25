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

public class RegisterTests(ControllerWebFactory webFactory) : ControllerTestsBase(webFactory)
{
    private ControllerWebFactory _webFactory = webFactory;

    [Fact]
    public async Task ShouldReturnSuccessAndSetAppropriateCookie_WhenCredentialsAreCorrect()
    {
        //Arange
        var registerForm = new RegisterAppUserForm("username", "email", "password", "password");
        var accessToken = new AccessTokenDto(registerForm.Username);

        _webFactory.AuthenticationService
        .RegisterAsync(registerForm, Arg.Any<CancellationToken>())
        .Returns(Result.Success(accessToken));
        //Act
        var response = await _client.PostAsJsonAsync("register?includeToken=true", registerForm);
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
        var registerForm = new RegisterAppUserForm("username", "email", "password", "password");
        var token = new AccessTokenDto(registerForm.Username);
        _webFactory.AuthenticationService
        .RegisterAsync(registerForm, Arg.Any<CancellationToken>())
        .Returns(Result.Success(token));
        //Act
        var response = await _client.PostAsJsonAsync("register?includeToken=true", registerForm);
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnNoContent_WhenIncludeTokenQueryParamIsAbsent()
    {
        //Arrange
        var registerForm = new RegisterAppUserForm("username", "email", "password", "password");
        var token = new AccessTokenDto(registerForm.Username);
        _webFactory.AuthenticationService
        .RegisterAsync(registerForm, Arg.Any<CancellationToken>())
        .Returns(Result.Success(token));
        //Act
        var response = await _client.PostAsJsonAsync("register", registerForm);
        //Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnNoContent_WhenIncludeTokenQueryParamIsFalse()
    {
        //Arrange
        var registerForm = new RegisterAppUserForm("username", "email", "password", "password");
        var token = new AccessTokenDto(registerForm.Username);
        _webFactory.AuthenticationService
        .RegisterAsync(registerForm, Arg.Any<CancellationToken>())
        .Returns(Result.Success(token));
        //Act
        var response = await _client.PostAsJsonAsync("register?includeToken=false", registerForm);
        //Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}