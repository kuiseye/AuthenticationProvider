using AuthenticationProvider.Application.Dtos.AppUserDtos;
using AuthenticationProvider.Application.Dtos.TokenDtos;

namespace AuthenticationProvider.Application.Services;

public interface IAuthenticationService
{
    public Task<Result<AccessTokenDto>> RegisterAsync(RegisterAppUserForm form, CancellationToken cancellationToken = default);
    public Task<Result<AccessTokenDto>> LoginAsync(LoginAppUserForm form, CancellationToken cancellationToken = default);
}