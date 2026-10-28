using AuthenticationProvider.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationProvider.Domain.Repositories;

public class AppUserRepository(ApplicationDbContext dbContext) : IAppUserRepository
{
    public async Task<AppUser?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
        .AsNoTracking()
        .Where(u=>u.Username == username)
        .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<AppUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
        .AsNoTracking()
        .Where(u=>u.Email == email)
        .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<List<AppUser>> GetUsersByUsernameOrEmailAsync(string username, string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
        .AsNoTracking()
        .Where(u=>u.Username == username || u.Email == email)
        .ToListAsync(cancellationToken);
    }

    public async Task<Guid> SaveUserAsync(AppUser appUser, CancellationToken cancellationToken = default)
    {
        await dbContext.Users.AddAsync(appUser, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return appUser.Id;
    }
}