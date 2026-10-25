using AuthenticationProvider.Domain.Repositories;
using AuthenticationProvider.Tests.Integration.Common;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationProvider.Tests.Integration.Domain.Repositories.AppUserRepository;

public class GetUsersByUsernameAndEmail : RepositoryTestsBase
{
    private IAppUserRepository _appUserRepository;
    public GetUsersByUsernameAndEmail(RepositoryWebFactory _webFactory) : base(_webFactory)
    {
        _appUserRepository = _scope.ServiceProvider.GetRequiredService<IAppUserRepository>();
    }

    [Theory]
    [InlineData("Oleg", "oleg@gmail.com", "hisPassword")]
    public async Task ShouldReturnOneUser_WhenEmailAndUsernameMatch(string username, string email, string password)
    {
        //Arrange
        await _dbContext.Users.AddAsync(AppUserFactory.CreateUser(username, email, password));
        await _dbContext.SaveChangesAsync();
        //Act
        var users = await _appUserRepository.GetUsersByUsernameOrEmailAsync(username, email);
        //Assert
        Assert.NotEmpty(users);
        Assert.True(users.Any(u=>u.Username == username) || users.Any(u=>u.Email == email));
        Assert.True(users.Count == 1);
    }

    [Theory]
    [InlineData("Oleg", "oleg@gmail.com", "hisPassword")]
    public async Task ShouldReturnTwoUsers_WhenEmailAndUsernameMatch(string username, string email, string password)
    {
        //Arrange
        await _dbContext.Users.AddAsync(AppUserFactory.CreateUser(username, "someEmail", password));
        await _dbContext.Users.AddAsync(AppUserFactory.CreateUser("someUsername", email, password));
        await _dbContext.SaveChangesAsync();
        //Act
        var users = await _appUserRepository.GetUsersByUsernameOrEmailAsync(username, email);
        //Assert
        Assert.NotEmpty(users);
        Assert.True(users.Any(u=>u.Username == username) || users.Any(u=>u.Email == email));
        Assert.True(users.Count == 2);
    }

    [Theory]
    [InlineData("Oleg", "oleg@gmail.com", "hisPassword")]
    public async Task ShouldReturnOneUser_WhenMatchedByEmail(string username, string email, string password)
    {
        //Arrange
        await _dbContext.Users.AddAsync(AppUserFactory.CreateUser("someUsername", email, password));
        await _dbContext.SaveChangesAsync();
        //Act
        var users = await _appUserRepository.GetUsersByUsernameOrEmailAsync("someOtherUsername", email);
        //Assert
        Assert.NotEmpty(users);
        Assert.Contains(users, u=>u.Email == email);
        Assert.DoesNotContain(users, u=>u.Username == username);
        Assert.True(users.Count == 1);
    }

    [Theory]
    [InlineData("Oleg", "oleg@gmail.com", "hisPassword")]
    public async Task ShouldReturnOneUser_WhenMatchedByUsername(string username, string email, string password)
    {
        //Arrange
        await _dbContext.Users.AddAsync(AppUserFactory.CreateUser(username, "someEmail", password));
        await _dbContext.SaveChangesAsync();
        //Act
        var users = await _appUserRepository.GetUsersByUsernameOrEmailAsync(username, email);
        //Assert
        Assert.NotEmpty(users);
        Assert.Contains(users, u=>u.Username == username);
        Assert.DoesNotContain(users, u=>u.Email == email);
        Assert.True(users.Count == 1);
    }

    [Theory]
    [InlineData("Oleg", "oleg@gmail.com")]
    public async Task ShouldReturnNull_WhenDoesntExist(string username, string email)
    {
        //Act
        var users = await _appUserRepository.GetUsersByUsernameOrEmailAsync(username, email);
        //Assert
        Assert.Empty(users);
    }
}