using System.Security.Claims;
using AuthenticationProvider.Domain.Entities;
using AuthenticationProvider.AppInterface.Common;

namespace AuthenticationProvider.Domain.Dtos.ClaimsDtos;

public class AppUserClaimsDto
{
    public Guid Id {get;}
    public string Username {get;}
    public AppUserRole Role {get;}

    public AppUserClaimsDto(ClaimsPrincipal claimsPrincipal)
    {
        var idString = claimsPrincipal.FindFirstValue(ApplicationClaimTypes.Id) 
            ?? throw new ArgumentNullException(nameof(claimsPrincipal), "Claims do not include Id field");
        if(!Guid.TryParse(idString, out Guid id)) throw new ArgumentException("Id inside claims is in invalid form");
        Id = id;

        Username = claimsPrincipal.FindFirstValue(ApplicationClaimTypes.Name) 
            ?? throw new ArgumentNullException(nameof(claimsPrincipal), "Claims do not include Name field");

        var roleString = claimsPrincipal.FindFirstValue(ApplicationClaimTypes.Role) 
            ?? throw new ArgumentNullException(nameof(claimsPrincipal), "Claims do not include Role field");
        if(!Enum.TryParse(roleString, out AppUserRole role)) throw new ArgumentException("Role inside claims is in invalid form");
        Role = role;
    }
}