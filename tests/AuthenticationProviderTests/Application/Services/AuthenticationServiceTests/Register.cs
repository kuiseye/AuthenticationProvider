using AuthenticationProvider.Application.Dtos.AppUserDtos;
using AuthenticationProvider.Application.Dtos.TokenDtos;
using AuthenticationProvider.Application.Errors;
using AuthenticationProvider.Domain.Entities;
using AuthenticationProvider.Tests.Unit.Application.Services.AuthenticationServiceTests;
using AuthenticationProvider.Tests.Unit.Common;
using NSubstitute;

namespace AuthenticationProvider.Tests.Application.Services.AuthenticationServiceTests;
public class Register : AuthenticationService_Common
{
    [Theory]
    [InlineData("Oleg", "oleg@gmail.com", "hisPassword", "hisPassword")]
    [InlineData("Pavlo", "pavlo@gmail.com", "myPassword", "myPassword")]
    [InlineData("Communism", "communism@gmail.com", "ourPassword", "ourPassword")]
    public async Task WhenCorrectCredentials_ShouldReturnSuccessAndSave
        (string username, string email, string password, string verifyPassword)
    {
        //Arrange
        _appUserRepository.GetUsersByUsernameOrEmailAsync(username,email).Returns([]);
        _tokenService.GenerateToken(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<AppUserRole>()).Returns(new AccessTokenDto(username));
        var registerForm = new RegisterAppUserForm(username,email,password,verifyPassword);
        //Act
        var result = await _authenticationService.RegisterAsync(registerForm);
        //Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.AccessToken == username);
        await _appUserRepository.Received().SaveUserAsync(Arg.Any<AppUser>());
    }

    [Theory]
    [InlineData("Oleg", "oleg@gmail.com", "hisPassword", "myPassword")]
    public async Task WhenWrongVerifyPasswordValue_ShouldReturnInvalidInputError
        (string username, string email, string password, string verifyPassword)
    {
        //Arrange
        _appUserRepository.GetUsersByUsernameOrEmailAsync(username,email).Returns([]);
        var registerForm = new RegisterAppUserForm(username,email,password, verifyPassword);
        //Act
        var result = await _authenticationService.RegisterAsync(registerForm);
        //Assert
        Assert.True(result.IsError);
        Assert.True(result.Error is InvalidInputError);
    }

    [Theory]
    [InlineData("Oleg", "oleg@gmail.com", "hisPassword", "hisPassword")]
    [InlineData("Pavlo", "pavlo@gmail.com", "myPassword", "myPassword")]
    [InlineData("Communism", "communism@gmail.com", "ourPassword", "ourPassword")]
    public async Task WhenEmailTaken_ShouldReturnValidationErrorOfEmailField
        (string username, string email, string password, string verifyPassword)
    {
        //Arrange
        _appUserRepository.GetUsersByUsernameOrEmailAsync(username,email).Returns([AppUserFactory.CreateUser("username", email, "password")]);
        var registerForm = new RegisterAppUserForm(username,email,password,verifyPassword);
        //Act
        var result = await _authenticationService.RegisterAsync(registerForm);
        //Assert
        Assert.True(result.IsError);
        var validationError = Assert.IsType<ValidationError>(result.Error);
        Assert.Contains(validationError.ValidationErrorEntries, valEntry => valEntry.Field==nameof(AppUser.Email));
    }

    [Theory]
    [InlineData("Oleg", "oleg@gmail.com", "hisPassword", "hisPassword")]
    [InlineData("Pavlo", "pavlo@gmail.com", "myPassword", "myPassword")]
    [InlineData("Communism", "communism@gmail.com", "ourPassword", "ourPassword")]
    public async Task WhenEmailTaken_ShouldReturnValidationErrorOfUsernameField
        (string username, string email, string password, string verifyPassword)
    {
        //Arrange
        _appUserRepository.GetUsersByUsernameOrEmailAsync(username,email).Returns([AppUserFactory.CreateUser(username, "email", "password")]);
        var registerForm = new RegisterAppUserForm(username,email,password,verifyPassword);
        //Act
        var result = await _authenticationService.RegisterAsync(registerForm);
        //Assert
        Assert.True(result.IsError);
        var validationError = Assert.IsType<ValidationError>(result.Error);
        Assert.Contains(validationError.ValidationErrorEntries, valEntry => valEntry.Field==nameof(AppUser.Username));
    }
}
