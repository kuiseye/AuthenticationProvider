using AuthenticationProvider.Application.Dtos.AppUserDtos;
using AuthenticationProvider.Application.Dtos.TokenDtos;
using AuthenticationProvider.Application.Services;
using AuthenticationProvider.Domain.Dtos.ClaimsDtos;
using AuthenticationProvider.AppInterface.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationProvider.AppInterface.Controllers;

[ApiController]
[Route("/")]
[Produces("application/json")]
[Consumes("application/json")]
public class AuthenticationController(IAuthenticationService authenticationService, ICookieManager cookieManager) : BaseController
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(AppUserDto), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 401)]
    
    public async Task<ActionResult<AppUserDto>> Me([FromServices] AppUserClaimsDto claims)
    {
        return new AppUserDto(claims.Username);
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AppUserDto), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 401)]
    public async Task<ActionResult<AccessTokenDto>> Register([FromBody] RegisterAppUserForm form,  CancellationToken cancellationToken, 
                                                             [FromQuery] bool includeToken = false)
    {
        var result = await _authenticationService.RegisterAsync(form, cancellationToken);
        result.IfPresent(value => cookieManager.AppendTokenToCookies(value.AccessToken));
        return result.Match(
            token => includeToken ? Ok(token) : Ok(null),
            MapErrors
        );
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AppUserDto), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 401)]
    public async Task<ActionResult<AccessTokenDto>> Login([FromBody] LoginAppUserForm form,  CancellationToken cancellationToken, 
                                                          [FromQuery] bool includeToken = false)
    {
        var result = await _authenticationService.LoginAsync(form, cancellationToken);
        result.IfPresent(value => cookieManager.AppendTokenToCookies(value.AccessToken));
        return result.Match(
            token => includeToken ? Ok(token) : Ok(null),
            MapErrors
        );
    }
}