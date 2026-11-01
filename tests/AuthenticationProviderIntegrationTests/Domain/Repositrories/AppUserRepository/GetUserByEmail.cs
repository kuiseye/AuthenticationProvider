using AuthenticationProvider.Domain.Repositories;
using AuthenticationProvider.Tests.Integration.Common;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationProvider.Tests.Integration.Domain.Repositories.AppUserRepository;

public class GetUserByEmail : RepositoryTestsBase
{
    private IAppUserRepository _appUserRepository;
    public GetUserByEmail(RepositoryWebFactory webFactory) : base(webFactory)
    {
        _appUserRepository = _scope.ServiceProvider.GetRequiredService<IAppUserRepository>();
    }


    [Theory]
    [InlineData("Oleg", "oleg@gmail.com", "hisPassword")]
    public async Task ShouldReturnSuccess_WhenExists(string username, string email, string password)
    {
        //Arrange
        await _dbContext.Users.AddAsync(AppUserFactory.CreateUser(username, email, password));
        await _dbContext.SaveChangesAsync();
        //Act
        var user = await _appUserRepository.GetUserByEmailAsync(email);
        //Assert
        Assert.NotNull(user);
        Assert.Equal(username, user.Username);
    }

    [Theory]
    [InlineData("Oleg", "oleg@gmail.com", "hisPassword")]
    public async Task ShouldReturnNull_WhenUsernameUsed(string username, string email, string password)
    {
        //Arrange
        await _dbContext.Users.AddAsync(AppUserFactory.CreateUser(username, email, password));
        await _dbContext.SaveChangesAsync();
        //Act
        var user = await _appUserRepository.GetUserByEmailAsync(username);
        //Assert
        Assert.Null(user);
    }

    [Theory]
    [InlineData("oleg@gmail.com")]
    public async Task ShouldReturnNull_WhenDoesntExist(string email)
    {
        //Act
        var user = await _appUserRepository.GetUserByEmailAsync(email);
        //Assert
        Assert.Null(user);
    }
}