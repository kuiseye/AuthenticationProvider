using AuthenticationProvider.Application.Services;
using AuthenticationProvider.Domain.Entities;
using AuthenticationProvider.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace AuthenticationProvider.Tests.Unit.Application.Services.AuthenticationServiceTests;

public class AuthenticationService_Common
{
    protected IAppUserRepository _appUserRepository = Substitute.For<IAppUserRepository>();
    protected ITokenService _tokenService = Substitute.For<ITokenService>();
    private IPasswordHasher<AppUser> _passwordHasher = new PasswordHasher<AppUser>();
    private IRequestClock requestClock = new RequestClock(TimeProvider.System);

    protected IAuthenticationService _authenticationService;

    public AuthenticationService_Common() 
        => _authenticationService = new AuthenticationService(_appUserRepository,_passwordHasher,_tokenService, requestClock);
}