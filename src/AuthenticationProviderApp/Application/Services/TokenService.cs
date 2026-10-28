using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthenticationProvider.Application.Dtos.TokenDtos;
using AuthenticationProvider.Configuration.Settings;
using AuthenticationProvider.Domain.Entities;
using AuthenticationProvider.AppInterface.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationProvider.Application.Services;

public class TokenService(SigningCredentials signingCredentials, IOptions<TokenSettings> tokenOptions, IRequestClock requestClock) 
    : ITokenService
{
    public AccessTokenDto GenerateToken(Guid id, string username, AppUserRole role)
    {
        Console.WriteLine("Sus " + requestClock.UtcNow);

        var claims = new List<Claim>()
        {
            new Claim(ApplicationClaimTypes.Id, id.ToString()),
            new Claim(ApplicationClaimTypes.Name, username),
            new Claim(ApplicationClaimTypes.Role, role.ToString())
        };
        var tokenSettings = tokenOptions.Value;
        var securityToken = new JwtSecurityToken(
            issuer: tokenSettings.Issuer,
            audience: tokenSettings.Audience,
            signingCredentials: signingCredentials,
            claims: claims,
            expires: requestClock.UtcNow.Add(tokenOptions.Value.Lifetime).UtcDateTime
        );
        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return new AccessTokenDto(token);
    }
}