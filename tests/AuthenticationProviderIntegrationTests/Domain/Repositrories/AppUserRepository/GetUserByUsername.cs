using AuthenticationProvider.Domain.Repositories;
using AuthenticationProvider.Tests.Integration.Common;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationProvider.Tests.Integration.Domain.Repositories.AppUserRepository;

public class GetUserByUsername : RepositoryTestsBase
{
    private IAppUserRepository _appUserRepository;
    public GetUserByUsername(RepositoryWebFactory _webFactory) : base(_webFactory)
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
        var user = await _appUserRepository.GetUserByUsernameAsync(username);
        //Assert
        Assert.NotNull(user);
        Assert.Equal(username, user.Username);
    }

    [Theory]
    [InlineData("Oleg", "oleg@gmail.com", "hisPassword")]
    public async Task ShouldReturnNull_WhenEmailUsed(string username, string email, string password)
    {
        //Arrange
        await _dbContext.Users.AddAsync(AppUserFactory.CreateUser(username, email, password));
        await _dbContext.SaveChangesAsync();
        //Act
        var user = await _appUserRepository.GetUserByUsernameAsync(email);
        //Assert
        Assert.Null(user);
    }

    [Theory]
    [InlineData("Oleg")]
    public async Task ShouldReturnNull_WhenDoesntExist(string username)
    {
        //Act
        var user = await _appUserRepository.GetUserByUsernameAsync(username);
        //Assert
        Assert.Null(user);
    }
}