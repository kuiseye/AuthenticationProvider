using AuthenticationProvider.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationProvider.Tests.Integration.Common;

public class AppUserFactory
{
    private static IPasswordHasher<AppUser> _passwordHasher = new PasswordHasher<AppUser>();
    public static AppUser CreateUser(string username, string email, string password)
    {
        return new AppUser(Guid.CreateVersion7(), 
        username, email, _passwordHasher.HashPassword(null!, password), 
        DateTimeOffset.UtcNow,
        DateTimeOffset.UtcNow,
        AppUserRole.User);
    }
}