using AuthenticationProvider.Domain.Entities;

namespace AuthenticationProvider.Domain.Repositories;

public interface IAppUserRepository
{
    public Task<AppUser?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);
    public Task<AppUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<List<AppUser>> GetUsersByUsernameOrEmailAsync(string username, string email, CancellationToken cancellationToken = default);

    public Task<Guid> SaveUserAsync(AppUser appUser, CancellationToken cancellationToken = default);
}