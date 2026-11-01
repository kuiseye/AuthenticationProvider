using AuthenticationProvider.Application.Dtos.TokenDtos;
using AuthenticationProvider.Domain.Entities;

namespace AuthenticationProvider.Application.Services;

public interface ITokenService
{
    public AccessTokenDto GenerateToken(Guid id, string username, AppUserRole role);
}