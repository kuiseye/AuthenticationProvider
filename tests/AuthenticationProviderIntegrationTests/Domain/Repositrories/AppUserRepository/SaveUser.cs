using AuthenticationProvider.Domain.Repositories;
using AuthenticationProvider.Tests.Integration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationProvider.Tests.Integration.Domain.Repositories.AppUserRepository;

public class SaveUser : RepositoryTestsBase
{
    private IAppUserRepository _appUserRepository;
    public SaveUser(RepositoryWebFactory _webFactory) : base(_webFactory)
    {
        _appUserRepository = _scope.ServiceProvider.GetRequiredService<IAppUserRepository>();
    }

    [Theory]
    [InlineData("Oleg","oleg@gmail.com","1234")]
    public async Task ShouldSave_WhenValidUser(string username, string email, string password)
    {
        //Arrange
        var user = AppUserFactory.CreateUser(username, email, password);
        //Act
        var id = await _appUserRepository.SaveUserAsync(user);
        //Arrange
        var retrievedUser = _dbContext.Users.Where(u=>u.Id == id).SingleOrDefault();
        Assert.NotNull(retrievedUser);
        Assert.Equal(user.Id, retrievedUser.Id);
        Assert.Equal(user.Username, retrievedUser.Username);
        Assert.Equal(user.Email, retrievedUser.Email);
    }

    [Theory]
    [InlineData("Oleg")]
    public async Task ShouldThrow_WhenNonUniqueUsername(string username)
    {
        //Arrange
        var originalUser = AppUserFactory.CreateUser(username, "someEmail", "somePassword");
        await _appUserRepository.SaveUserAsync(originalUser);
        var newUser = AppUserFactory.CreateUser(username, "someOtherEmail", "somePassword");
        //Act/Assert
        await Assert.ThrowsAsync<DbUpdateException>(async ()=>{
            await _appUserRepository.SaveUserAsync(newUser);
        });
    }

    [Theory]
    [InlineData("Oleg")]
    public async Task ShouldThrow_WhenNonUniqueEmail(string email)
    {
        //Arrange
        var originalUser = AppUserFactory.CreateUser("someUsername", email, "somePassword");
        await _appUserRepository.SaveUserAsync(originalUser);
        var newUser = AppUserFactory.CreateUser("someUsername", email, "somePassword");
        //Act/Assert
        await Assert.ThrowsAsync<DbUpdateException>(async ()=>{
            await _appUserRepository.SaveUserAsync(newUser);
        });
    }
}