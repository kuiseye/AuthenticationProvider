using AuthenticationProvider.Application.Dtos.AppUserDtos;
using AuthenticationProvider.Application.Dtos.TokenDtos;
using AuthenticationProvider.Application.Errors;
using AuthenticationProvider.Domain.Entities;
using AuthenticationProvider.Tests.Unit.Common;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace AuthenticationProvider.Tests.Unit.Application.Services.AuthenticationServiceTests;

public class Login : AuthenticationService_Common
{
    [Theory]
    [InlineData("Oleg", "hisPassword")]
    [InlineData("Pavlo","myPassword")]
    [InlineData("Communism", "ourPassword")]
    public async Task WhenCorrectCredentials_ShouldReturnSuccess
        (string username, string password)
    {
        //Arrange
        _appUserRepository.GetUserByUsernameAsync(username).Returns(AppUserFactory.CreateUser(username, "email" + username, password));
        _tokenService.GenerateToken(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<AppUserRole>()).Returns(new AccessTokenDto(username));
        var loginForm = new LoginAppUserForm(username, password);
        //Act
        var result = await _authenticationService.LoginAsync(loginForm);
        //Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.AccessToken == username);
    }

    [Theory]
    [InlineData("Oleg", "hisPassword")]
    [InlineData("Pavlo","myPassword")]
    [InlineData("Communism", "ourPassword")]
    public async Task UserNotFound_ShouldReturnFailureWithUnauthorizedError
        (string username, string password)
    {
        //Arrange
        _appUserRepository.GetUserByUsernameAsync(username).ReturnsNull();
        var loginForm = new LoginAppUserForm(username, password);
        //Act
        var result = await _authenticationService.LoginAsync(loginForm);
        //Assert
        Assert.True(result.IsError);
        Assert.IsType<UnauthorizedError>(result.Error);
    }

    [Theory]
    [InlineData("Oleg", "hisPassword")]
    [InlineData("Pavlo","myPassword")]
    [InlineData("Communism", "ourPassword")]
    public async Task PasswordDoesNotmatch_ShouldReturnFailureWithUnauthorizedError
        (string username, string password)
    {
        //Arrange
        _appUserRepository.GetUserByUsernameAsync(username).Returns(AppUserFactory.CreateUser(username, "email" + username, "some_secret_password"));
        var loginForm = new LoginAppUserForm(username, password);
        //Act
        var result = await _authenticationService.LoginAsync(loginForm);
        //Assert
        Assert.True(result.IsError);
        Assert.IsType<UnauthorizedError>(result.Error);
    }
}